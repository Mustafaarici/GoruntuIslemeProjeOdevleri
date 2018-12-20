const int motorPin1 = 8;
const int motorPin2 = 9;
const int motorPin3 = 10;
const int motorPin4 = 11;
char c; // Seri porttan okunan veri.
int sayac=0; // Motor başlangıç pozisyonu için tanımlanmıştır.

void adim1() {
digitalWrite(motorPin1, HIGH);
digitalWrite(motorPin2, LOW);
digitalWrite(motorPin3, LOW);
digitalWrite(motorPin4, LOW);
delay(20);
}
void adim2() {
digitalWrite(motorPin1, LOW);
digitalWrite(motorPin2, HIGH);
digitalWrite(motorPin3, LOW);
digitalWrite(motorPin4, LOW);
delay(20);
}
void adim3() {
digitalWrite(motorPin1, LOW);
digitalWrite(motorPin2, LOW);
digitalWrite(motorPin3, HIGH);
digitalWrite(motorPin4, LOW);
delay(20);
}
void adim4() {
digitalWrite(motorPin1, LOW);
digitalWrite(motorPin2, LOW);
digitalWrite(motorPin3, LOW);
digitalWrite(motorPin4, HIGH);
delay(20);
}
void setup() {
pinMode(motorPin1, OUTPUT);
pinMode(motorPin2, OUTPUT);
pinMode(motorPin3, OUTPUT);
pinMode(motorPin4, OUTPUT);
Serial.begin(9600);
}

void loop() {
if(Serial.available() > 0) // Seriport'ta bir aktarım var ise.
{
  c = Serial.read(); // C#'tan gönderilen veriler burada okunur.

if (c=='a') // Sağa dön.
{
adim4();
adim3();
adim2();
adim1();
sayac++;
}
if (c=='c') // Sola dön.
{
adim1();
adim2();
adim3();
adim4();
sayac--;
}
if(c=='d') // Hareket etme.
{
digitalWrite(motorPin1, LOW);
digitalWrite(motorPin2, LOW);
digitalWrite(motorPin3, LOW);
digitalWrite(motorPin4, LOW);
delay(20);
}
if(c=='b' && sayac>0) //Başlangıç pozisyonuna dön.Eğer sayac pozitifse.
{
    for(int i=0 ; i<sayac ; i++)
    {
      adim1();
      adim2();
      adim3();
      adim4();
    }
    sayac = 0;
  }
  if(c=='b' && sayac<0 ) //Başlangıç pozisyonuna dön.Eğer sayac negatifse.
  {
    sayac = sayac * (-1);
    for(int i=0 ; i<sayac ; i++)
    {
     adim4();
     adim3();
     adim2();
     adim1();
    }
    sayac = 0;
  }
}
}
