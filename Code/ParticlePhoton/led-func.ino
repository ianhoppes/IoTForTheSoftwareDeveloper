int greenLedPin = D2;
int redLedPin = D4;
int lightLed(String color);

void setup()
{
    pinMode(greenLedPin, OUTPUT);
    pinMode(redLedPin, OUTPUT);
    Particle.function("lightLed", lightLed);
    lightLed("green");
}

void loop()
{
  // this loops forever
}

int lightLed(String color)
{
    if (color == "red") {
        digitalWrite(redLedPin, HIGH);
        digitalWrite(greenLedPin, LOW);
        return 1;
    }
    else if (color == "green") {
        digitalWrite(redLedPin, LOW);
        digitalWrite(greenLedPin, HIGH);
        return 1;
    }
    else
        return -1;
}