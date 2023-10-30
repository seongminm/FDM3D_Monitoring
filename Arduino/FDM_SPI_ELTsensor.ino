// master
#include <SPI.h> // master - slave SPI 통신 라이브러리

const int SLAVE_PIN1 = 2;
const int SLAVE_PIN2 = 3;
const int SLAVE_PIN3 = 4;
const int SLAVE_PIN4 = 5;

void setup() {
  Serial.begin(9600);
  pinMode(11, OUTPUT);
  pinMode(12, INPUT);
  pinMode(13, OUTPUT);
  pinMode(SLAVE_PIN1,OUTPUT);
  pinMode(SLAVE_PIN2,OUTPUT);
  pinMode(SLAVE_PIN3,OUTPUT);
  pinMode(SLAVE_PIN4,OUTPUT);
  digitalWrite(SLAVE_PIN1, HIGH); // 슬레이브 해제
  digitalWrite(SLAVE_PIN2, HIGH); // 슬레이브 해제
  digitalWrite(SLAVE_PIN3, HIGH); // 슬레이브 해제
  digitalWrite(SLAVE_PIN4, HIGH); // 슬레이브 해제
  SPI.begin();
  SPI.setClockDivider(SPI_CLOCK_DIV8); 

}


void loop() {
  // 슬레이브 1 통신
  String data1 = "";
  digitalWrite(SLAVE_PIN1, LOW); 
  delay(100);
  while(1) {
    char received1 = SPI.transfer(0);
    if(received1 == '\0') {
      break;
    }  else {
      data1 += received1;
    }
  }
  digitalWrite(SLAVE_PIN1, HIGH);
  if(data1 != "") {
    Serial.print("slave 1 :");
    Serial.println(data1);
  }
  

  // 슬레이브 2 통신
  String data2 = "";
  digitalWrite(SLAVE_PIN2, LOW); 
  delay(100);
  while(1) {
    char received2 = SPI.transfer(0);
    if(received2 == '\0') {
      break;
    } else {
      data2 += received2;
    }
  }
  digitalWrite(SLAVE_PIN2, HIGH);
  if(data2 != "") {
    Serial.print("slave 2 :");
    Serial.println(data2);
  }
  

  // 슬레이브 3 통신
  String data3 = "";
  digitalWrite(SLAVE_PIN3, LOW); 
  delay(100);
  while(1) {
    char received3 = SPI.transfer(0);
    if(received3 == '\0') {
      break;
    } else {
      data3 += received3;
    }
  }
  digitalWrite(SLAVE_PIN3, HIGH);
  if(data3 != "") {
    Serial.print("slave 3 :");
    Serial.println(data3);
  }

  // 슬레이브 4 통신
  String data4 = "";
  digitalWrite(SLAVE_PIN4, LOW); 
  delay(100);
  while(1) {
    char received4 = SPI.transfer(0);
    if(received4 == '\0') {
      break;
    } else {
      data4 += received4;
    }
  }
  digitalWrite(SLAVE_PIN4, HIGH);
  if(data4 != "") {
    Serial.print("slave 4 :");
    Serial.println(data4);
  }
}

===================================================
//slave

#include <DHT.h>  // DHT22 센서용 라이브러리
#include <SoftwareSerial.h>
#include <Wire.h> // I2C 통신 라이브러리
#include <SPI.h> // master - slave SPI 통신 라이브러리

#define slaveNumber 'A' // A, B, C.....H 

#define DHTPIN 9           // DHT센서값을 받을 디지털핀
#define DHTTYPE DHT22      // 사용할 센서타입을 DHT22 로 지정
DHT dht(DHTPIN, DHTTYPE);  // 센서 객체 생성

#define PIN_TX_PMS7003 7  // PIN matched with TX of PMS7003
#define PIX_RX_PMS7003 6   // PIN matched with RX of PMS7003

#define HEAD_1 0x42 // pms Checksum
#define HEAD_2 0x4d // pms Checksum

#define PMS7003_BAUD_RATE 9600  // Serial Speed of PMS7003

SoftwareSerial pmsSerial(PIN_TX_PMS7003, PIX_RX_PMS7003);  // RX, TX of Arduino UNO
unsigned char pmsbytes[32];                                

// VOC 센서 변경 시, 센서에 맞게 sensor address 주석 변경
// ELT VOC Sensor address
#define SLAVE_ADDRESS 0x59
#define SLAVE_ADDRESS_WRITE 0xB2
#define SLAVE_ADDRESS_READ 0xB3

// ELT H2S Sensor address 
// #define SLAVE_ADDRESS 0x72
// #define SLAVE_ADDRESS_WRITE 0xE4
// #define SLAVE_ADDRESS_READ 0xE5

// ELT NH3 Sensor address 
// #define SLAVE_ADDRESS 0x73
// #define SLAVE_ADDRESS_WRITE 0xE6
// #define SLAVE_ADDRESS_READ 0xE7

unsigned long previousTime1 = 0;       // 첫 번째 함수 호출 시간 기록
unsigned long previousTime2 = 0;       // 두 번째 함수 호출 시간 기록
const unsigned long interval1 = 1000;  // 첫 번째 함수 호출 주기 (1000ms)
const unsigned long interval2 = 2000;  // 두 번째 함수 호출 주기 (2000ms)

uint16_t pm1_0;
uint16_t pm2_5;
uint16_t pm10;

float h;
float t;
float Vol[4] = {
  0,
};

String getI2c;

String stringData;
int bufferSize;

void setup() {
  // put your setup code here, to run once:
  stringData = "";
  bufferSize = 0;
  Wire.begin();
  pmsSerial.begin(PMS7003_BAUD_RATE);
  dht.begin();

  pinMode(10, INPUT);
  pinMode(11, INPUT);
  pinMode(13, INPUT);
  pinMode(12, OUTPUT); 

  SPI.setClockDivider(SPI_CLOCK_DIV8); 
  SPCR |=_BV(SPE);
  SPCR &= ~_BV(MSTR);
}

void loop() {
  // put your main code here, to run repeatedly:
  unsigned long currentTime = millis();  // 현재 시간 읽기
  if (pmsSerial.available() >= 32) {
    int i = 0;

    //initialize first two bytes with 0x00
    pmsbytes[0] = 0x00;
    pmsbytes[1] = 0x00;

    for (i = 0; i < 32; i++) {
      pmsbytes[i] = pmsSerial.read();

      //check first two bytes - HEAD_1 and HEAD_2, exit when it's not normal and read again from the start
      if ((i == 0 && pmsbytes[0] != HEAD_1) || (i == 1 && pmsbytes[1] != HEAD_2)) {
        break;
      }
    }

    if (i > 2) {                                     // only when first two stream bytes are normal
      if (pmsbytes[29] == 0x00) {                    // only when stream error code is 0
        pm1_0 = (pmsbytes[10] << 8) | pmsbytes[11];  // pmsbytes[10]:HighByte + pmsbytes[11]:LowByte => two bytes
        pm2_5 = (pmsbytes[12] << 8) | pmsbytes[13];  // pmsbytes[12]:HighByte + pmsbytes[13]:LowByte => two bytes
        pm10 = (pmsbytes[14] << 8) | pmsbytes[15];   // pmsbytes[14]:HighByte + pmsbytes[15]:LowByte => two bytes

      } else {
        // Serial.println("Error skipped..");
      }
    } else {
      // Serial.println("Bad stream format error");
    }
  }

  if (currentTime - previousTime1 >= interval1) {
    Wire.beginTransmission(SLAVE_ADDRESS);
    Wire.write(SLAVE_ADDRESS_WRITE);
    Wire.write(0x52);
    Wire.endTransmission();

    Wire.beginTransmission(SLAVE_ADDRESS);
    Wire.write(SLAVE_ADDRESS_READ);
    Wire.endTransmission();

    Wire.requestFrom(SLAVE_ADDRESS, 7);  // 7바이트 데이터 요청
    if (Wire.available() >= 7) {
      byte data[7];
      for (int i = 0; i < 7; i++) {
        data[i] = Wire.read();
      }
      int value = (data[1] << 8) | data[2];
      getI2c = String(value);
    }
    previousTime1 = currentTime;  // 다음 호출 시간 갱신
  }

  if (currentTime - previousTime2 >= interval2) {
    h = dht.readHumidity();     // 습도값 요청
    t = dht.readTemperature();  // 온도값 요청

    for(int i = 0; i < 4; i++) {
      Vol[i] = (analogRead(i) * 5.0 )/ 1024.0;
    }
    
    stringData = String(slaveNumber) + "/"
    + String(h) + "/" 
    + String(t) + "/" 
    + String(pm1_0) + "/"
    + String(pm2_5) + "/"
    + String(pm10) + "/"
    + String(getI2c) + "/"
    + String(Vol[2]) + "/"
    + String(Vol[1]) + "/"
    + String(Vol[0]) + "/"
    + String(Vol[3]) + '\0';
    
    bufferSize = stringData.length()+1;
    previousTime2 = currentTime;  // 다음 호출 시간 갱신
  }

  if(digitalRead(10) == LOW) {
    byte buffer[bufferSize];

    stringData.toCharArray(buffer, bufferSize);
    
    for(int i = 0; i < bufferSize; i++) {
      SPI.transfer(buffer[i]);
    }      
  } 
    // slaveNumber / Humidity / Temperature / pm1.0 / pm2.5 / pm10 / ELT / Mics / CJMCU / MQ / HCHO 
}
