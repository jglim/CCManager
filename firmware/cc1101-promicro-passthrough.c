#include "SPI.h";

#define SPI_SS   10
#define SPI_MOSI 11
#define SPI_MISO 12
#define SPI_SCK  13

void setup() 
{
	Serial.begin(9600);
	setupSPI();}

/*
 * Command bytes:
 * a - Reset RF
 * b - delay, delayTime (ms)
 * c - select chip (SS)
 * d - deselect chip (SS)
 * e - wait MISO low
 * f - SPI transfer, transferByte
 */

char inputBuffer[5];
byte bufferIndex = 0;
bool isReading = false;

void loop() 
{
  
  if (Serial.available() > 0) 
  {
    char inChar = Serial.read();

    // Filter all requests by having first char start with '{' and last char with '}'
    // Prevents serial garbage from affecting the parser (e.g. other app probing all serial ports)
    if (!isReading)
    {
      if (inChar == '{')
      {
        isReading = true;
      }
      else 
      {
        return;
      }
    }
    // read incoming serial data:
    inputBuffer[bufferIndex] = inChar;
    bufferIndex++;

    if (bufferIndex == 5)
    {
      bufferIndex = 0;
    }


    if (inChar == '}')
    {
      isReading = false;
      bufferIndex = 0;

      if (inputBuffer[4] != '}')
      {
        // command is not terminated correctly
        Serial.print("z0");
      }
      else 
      {
        byte command = inputBuffer[1];
        byte firstParameter = (convertCharToByte(inputBuffer[2]) << 4) | convertCharToByte(inputBuffer[3]);

        if (command == 'a')
        {
          resetRf();
          Serial.println("a1");
        }
        else if (command == 'b')
        {
          delay(firstParameter);
          Serial.println("b1");
        }
        else if (command == 'c')
        {
          cc1101_Select();
          Serial.println("c1");
        }
        else if (command == 'd')
        {
          cc1101_Deselect();
          Serial.println("d1");
        }
        else if (command == 'e')
        {
          wait_Miso();
          Serial.println("e1");
        }
        else if (command == 'f')
        {
          SPI.transfer(firstParameter);
          Serial.println("f1");
        }
        else if (command == 'g')
        {
          Serial.println("ATMEGA328P PRO MINI 0.1");
        }
      }
    }
  }  

}


// converts an ascii character/number to the byte equivalent
// does not perform any validation
byte convertCharToByte(char inputChar)
{
  byte result = (int)inputChar;
  // 57: ascii '9'
  if (result > 57)
  {
    return result - 55;
  }
  else 
  {
    return result - 48;
  }
}

// Reset CC1101 (requires implementation here since microsecond delay can't be done reliably via serial)
void resetRf()
{
	cc1101_Deselect(); 
	delayMicroseconds(5);
  cc1101_Select();
	delayMicroseconds(10);
	cc1101_Deselect();
	delayMicroseconds(41);
	cc1101_Select();

  wait_Miso();
	SPI.transfer(0x30); // SRES
  wait_Miso();

	cc1101_Deselect();
}

void wait_Miso()
{
  while(digitalRead(SPI_MISO));
}

void cc1101_Select()  
{
  digitalWrite(SPI_SS, LOW);
}

void cc1101_Deselect()  
{
  digitalWrite(SPI_SS, HIGH);
}

void setupSPI()
{
  pinMode(SPI_SS, OUTPUT);
  pinMode(SPI_MISO, INPUT);
  digitalWrite(SPI_SS, HIGH);
  digitalWrite(SPI_SCK, HIGH);
  digitalWrite(SPI_MOSI, LOW);

  SPI.begin();
  SPI.setBitOrder(MSBFIRST);
  SPI.setDataMode(SPI_MODE0);
  //SPI.setClockDivider(SPI_CLOCK_DIV64);
}
