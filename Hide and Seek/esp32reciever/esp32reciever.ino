
#include <TFT_eSPI.h>
#include <TJpg_Decoder.h>

TFT_eSPI tft = TFT_eSPI();

#define TFT_BL 27

const int WIDTH = 480;
const int HEIGHT = 320;
const int MAX_JPEG_SIZE = 32768;

// Backlight brightness: 0 = off, 255 = full brightness
const int BRIGHTNESS = 255;

const uint8_t MAGIC[] = {0xAA, 0x55, 0xAA, 0x55};

uint8_t* jpegBuffer;

bool tftOutput(int16_t x, int16_t y, uint16_t w, uint16_t h, uint16_t* bitmap)
{
    if (x >= WIDTH || y >= HEIGHT)
        return false;

    if (x + w > WIDTH)
        w = WIDTH - x;

    if (y + h > HEIGHT)
        h = HEIGHT - y;

    if (w == 0 || h == 0)
        return true;

    tft.setAddrWindow(x, y, w, h);
    tft.pushPixels(bitmap, w * h);

    return true;
}

bool findMagic()
{
    static int magicIndex = 0;

    while (Serial.available() > 0)
    {
        uint8_t byte = Serial.read();

        if (byte == MAGIC[magicIndex])
        {
            magicIndex++;

            if (magicIndex == 4)
            {
                magicIndex = 0;
                return true;
            }
        }
        else
        {
            magicIndex = 0;

            if (byte == MAGIC[0])
                magicIndex = 1;
        }
    }

    return false;
}

bool readExact(uint8_t* buffer, uint32_t length)
{
    uint32_t received = 0;
    unsigned long lastDataTime = millis();

    while (received < length)
    {
        int available = Serial.available();

        if (available > 0)
        {
            uint32_t remaining = length - received;

            if ((uint32_t)available > remaining)
                available = remaining;

            int bytesRead = Serial.readBytes(buffer + received, available);

            if (bytesRead > 0)
            {
                received += bytesRead;
                lastDataTime = millis();
            }
        }

        if (millis() - lastDataTime > 2000)
            return false;

        yield();
    }

    return true;
}

void setup()
{
    Serial.begin(115200);
    Serial.setTimeout(100);

    pinMode(TFT_BL, OUTPUT);

    // Set backlight brightness
    analogWrite(TFT_BL, BRIGHTNESS);

    tft.begin();
    tft.setRotation(1);
    tft.fillScreen(TFT_BLACK);

    TJpgDec.setCallback(tftOutput);
    TJpgDec.setJpgScale(1);
    TJpgDec.setSwapBytes(true);

    jpegBuffer = (uint8_t*)malloc(MAX_JPEG_SIZE);

    if (jpegBuffer == nullptr)
    {
        tft.fillScreen(TFT_RED);
        return;
    }
}

void loop()
{
    if (jpegBuffer == nullptr)
        return;

    if (!findMagic())
        return;

    uint8_t sizeBytes[4];

    if (!readExact(sizeBytes, 4))
    {
        Serial.write('T');
        Serial.flush();
        return;
    }

    uint32_t frameSize = 0;

    frameSize |= (uint32_t)sizeBytes[0];
    frameSize |= (uint32_t)sizeBytes[1] << 8;
    frameSize |= (uint32_t)sizeBytes[2] << 16;
    frameSize |= (uint32_t)sizeBytes[3] << 24;

    if (frameSize == 0 || frameSize > MAX_JPEG_SIZE)
    {
        Serial.write('S');
        Serial.flush();
        return;
    }

    if (!readExact(jpegBuffer, frameSize))
    {
        Serial.write('T');
        Serial.flush();
        return;
    }

    if (frameSize < 2 || jpegBuffer[0] != 0xFF || jpegBuffer[1] != 0xD8)
    {
        Serial.write('H');
        Serial.flush();
        return;
    }

    tft.startWrite();

    JRESULT result = TJpgDec.drawJpg(0, 0, jpegBuffer, frameSize);

    tft.endWrite();

    Serial.write((uint8_t)('0' + (int)result));
    Serial.flush();
}
