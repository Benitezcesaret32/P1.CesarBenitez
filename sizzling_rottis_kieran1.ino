// C++ code
//
#define PinPotenciometro A0
#define PinTemperatura A1
#define PinLuz A2

#define PinPIR 8

#define LedRojo 2
#define LedVerde 4
#define LedAmarillo 7

void setup()
{
  pinMode(LedRojo, OUTPUT);
  pinMode(LedVerde, OUTPUT);
  pinMode(LedAmarillo, OUTPUT);

  pinMode(PinPIR, INPUT);

  Serial.begin(9600);
}

void loop()
{
  int valorPot = map(analogRead(PinPotenciometro), 0, 1023, 0, 100);

  float voltaje = analogRead(PinTemperatura) * 5.0 / 1023.0;
  float grados = (voltaje - 0.5) * 100;

  int movimiento = digitalRead(PinPIR);

  int luz = map(analogRead(PinLuz), 0, 1023, 0, 100);

  if (luz == 100)
  {
    digitalWrite(LedRojo, HIGH);
    digitalWrite(LedVerde, HIGH);
    digitalWrite(LedAmarillo, HIGH);
  }
  else
  {
    digitalWrite(LedRojo, LOW);
    digitalWrite(LedVerde, LOW);
    digitalWrite(LedAmarillo, LOW);

    if (valorPot >= 50 && valorPot <= 75 && grados < 15 && movimiento == HIGH)
    {
      digitalWrite(LedRojo, HIGH);
      delay(1000);
      digitalWrite(LedRojo, LOW);

      digitalWrite(LedVerde, HIGH);
      delay(1000);
      digitalWrite(LedVerde, LOW);

      digitalWrite(LedAmarillo, HIGH);
      delay(1000);
      digitalWrite(LedAmarillo, LOW);
    }
  }
}
