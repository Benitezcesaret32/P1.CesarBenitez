#include <Adafruit_LiquidCrystal.h>
#include <Servo.h>

#define pinTemp A0
#define pinPot A1
#define pinServo 3
#define pinRojo 2
#define pinVerde 4
#define pinAzul 5
#define pinBoton 8
#define pinBuzzer 10

#define ARRAY_LEN(array) (sizeof(array) / sizeof(array[0]))
#define Gb3 208
#define C4 262
#define F3 175
#define A3 220

const int midi1[10][3] = {
 {Gb3, 115, 0},
 {C4, 115, 0},
 {Gb3, 115, 0},
 {F3, 115, 0},
 {A3, 115, 115},
 {Gb3, 115, 0},
 {C4, 115, 0},
 {Gb3, 115, 0},
 {F3, 115, 0},
 {A3, 115, 0},
};

void playMidi(int pin, const int notes[][3], size_t len){
 for (int i = 0; i < len; i++) {
    tone(pin, notes[i][0]);
    delay(notes[i][1]);
    noTone(pin);
    delay(notes[i][2]);
  }
}

int temperatura;
int potenciometro;
int espera;

Servo servo1;

Adafruit_LiquidCrystal lcd1(0);

void setup()
{
  pinMode(pinRojo, OUTPUT);
  pinMode(pinVerde, OUTPUT);
  pinMode(pinAzul, OUTPUT);
  pinMode(pinBuzzer, OUTPUT);
  pinMode(pinBoton, INPUT);

  servo1.attach(pinServo);
  servo1.write(0);

  Serial.begin(9600);

  lcd1.begin(16,2);
}

void loop()
{
  chequearTemp();

  if(digitalRead(pinBoton) == LOW){
    moverServo();
    tocarMelodia();
  }

  delay(200);
}

void moverServo(){

  lcd1.clear();
  lcd1.setCursor(0,0);
  lcd1.print("Servo moviendose");

  potenciometro = analogRead(pinPot);

  if(potenciometro < 256){
    espera = 2000;
  }else if(potenciometro <= 512){
    espera = 5000;
  }else{
    espera = 8000;
  }

  for(int i = 0; i < 10; i++){

    servo1.write(90);
    delay(espera);

    servo1.write(0);
    delay(espera);
  }
}

void tocarMelodia(){

  lcd1.clear();
  lcd1.setCursor(0,0);
  lcd1.print("Melodia");

  digitalWrite(pinVerde, HIGH);

  playMidi(pinBuzzer, midi1, ARRAY_LEN(midi1));

  digitalWrite(pinVerde, LOW);
}

void chequearTemp(){

  temperatura = analogRead(pinTemp);

  temperatura = map(((temperatura - 20) * 3.04), 0, 1023, -40, 125);

  if(temperatura > 40){
    digitalWrite(pinRojo, HIGH);
    digitalWrite(pinAzul, LOW);
  }else{
    digitalWrite(pinRojo, LOW);
    digitalWrite(pinAzul, HIGH);
  }
}