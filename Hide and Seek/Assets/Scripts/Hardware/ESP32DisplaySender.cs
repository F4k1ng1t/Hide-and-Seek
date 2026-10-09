using System;
using System.IO.Ports;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class ESP32DisplaySender : MonoBehaviour
{
    [Header("Display")]
    public RenderTexture displayTexture;
    public int width = 480;
    public int height = 320;

    [Range(1, 100)]
    public int jpegQuality = 24;

    [Header("Serial")]
    public string portName = "COM6";
    public int baudRate = 2000000;
    public int targetFPS = 10;

    SerialPort serialPort;

    float frameTimer;
    bool waitingForESP32;
    bool readbackPending;

    static readonly byte[] Magic = { 0xAA, 0x55, 0xAA, 0x55 }; //tf????
    readonly byte[] sizeBytes = new byte[4];

    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.ReadTimeout = 100;
        serialPort.WriteTimeout = 2000;

        try
        {
            serialPort.Open();

            serialPort.DiscardInBuffer();
            serialPort.DiscardOutBuffer();

            Debug.Log($"Connected to ESP32 on {portName} at {baudRate} baud");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to open {portName}: {e.Message}");
        }
    }

    void Update()
    {
        Debug.Log("Check 1");
        if (serialPort == null || !serialPort.IsOpen)
            return;

        Debug.Log("Check 2");
        if (displayTexture == null)
            return;

        Debug.Log("Check 3");
        CheckESP32();

        Debug.Log("Waiting: " + waitingForESP32);
        Debug.Log("readback: " + readbackPending);
        if (waitingForESP32 || readbackPending)
            return;

        Debug.Log("Check 4");
        frameTimer += Time.deltaTime;

        if (frameTimer >= 1f / targetFPS)
        {
            Debug.Log("Rendered a fucking frame");
            frameTimer = 0f;
            RequestFrame();
        }
    }

    void RequestFrame()
    {
        readbackPending = true;

        AsyncGPUReadback.Request(displayTexture, 0, TextureFormat.RGB24, OnReadbackComplete);
    }

    void OnReadbackComplete(AsyncGPUReadbackRequest request)
    {
        readbackPending = false;

        if (request.hasError)
        {
            Debug.LogError("GPU readback failed.");
            return;
        }

        NativeArray<byte> pixelData = request.GetData<byte>();

        NativeArray<byte> jpegData = ImageConversion.EncodeNativeArrayToJPG(
            pixelData,
            UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8_UNorm,
            (uint)width,
            (uint)height,
            (uint)(width * 3),
            jpegQuality
        );

        if (!jpegData.IsCreated || jpegData.Length == 0)
        {
            Debug.LogError("Failed to encode JPEG.");
            return;
        }

        if (jpegData.Length > 32768)
        {
            Debug.LogError($"JPEG too large: {jpegData.Length} bytes");
            jpegData.Dispose();
            return;
        }

        int frameSize = jpegData.Length;

        sizeBytes[0] = (byte)(frameSize & 0xFF);
        sizeBytes[1] = (byte)((frameSize >> 8) & 0xFF);
        sizeBytes[2] = (byte)((frameSize >> 16) & 0xFF);
        sizeBytes[3] = (byte)((frameSize >> 24) & 0xFF);

        try
        {
            serialPort.Write(Magic, 0, Magic.Length);
            serialPort.Write(sizeBytes, 0, sizeBytes.Length);

            byte[] jpegBytes = jpegData.ToArray();

            serialPort.Write(jpegBytes, 0, jpegBytes.Length);

            waitingForESP32 = true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to send frame: {e.Message}");
            waitingForESP32 = false;
        }
        finally
        {
            jpegData.Dispose();
        }
    }

    void CheckESP32()
    {
        while (serialPort.BytesToRead > 0)
        {
            Debug.Log("X Check 0");
            int response = serialPort.ReadByte();

            if (response >= '0' && response <= '8')
            {
                Debug.Log("X Check 1");
                int result = response - '0';

                if (result == 0)
                {
                    waitingForESP32 = false;
                }
                else
                {
                    Debug.LogError($"ESP32 JPEG decoder error: JDR_{result} - {GetDecoderErrorName(result)}");
                    waitingForESP32 = false;
                }
            }
            else if (response == 'T')
            {
                Debug.LogError("ESP32: JPEG receive timeout.");
                waitingForESP32 = false;
            }
            else if (response == 'S')
            {
                Debug.LogError("ESP32: Invalid JPEG size.");
                waitingForESP32 = false;
            }
            else if (response == 'H')
            {
                Debug.LogError("ESP32: Invalid JPEG header.");
                waitingForESP32 = false;
            }
        }
    }

    string GetDecoderErrorName(int errorCode)
    {
        switch (errorCode)
        {
            case 0:
                return "OK";
            case 1:
                return "Interrupted by output function";
            case 2:
                return "Input/device error or wrong termination";
            case 3:
                return "Insufficient memory pool";
            case 4:
                return "Insufficient stream input buffer";
            case 5:
                return "Parameter error";
            case 6:
                return "Data format error / possibly corrupted JPEG";
            case 7:
                return "JPEG format is valid but unsupported";
            case 8:
                return "Unsupported JPEG standard";
            default:
                return "Unknown error";
        }
    }

    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
            serialPort.Close();
    }
}