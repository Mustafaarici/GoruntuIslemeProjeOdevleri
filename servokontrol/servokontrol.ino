
#include <Servo.h> // Servomotor kütüphanesi.
Servo myservo;
Servo myservo2;   
int t; //Seri porttan okunan değişken
void setup() {
  myservo.attach(9); 
    myservo2.attach(10);
  myservo.write(0); // Başlangıç pozisyonları sıfır olarak belirlendi.
    myservo2.write(0);
   Serial.begin(9600);
}
  
void loop() {

   if (Serial.available() > 0)
   {
      t=Serial.read(); // Seriporttan okunan deger.
      if(t== 'd')
      {
        myservo.write(0);  
        myservo2.write(0);  
      }
      if(t == '1') // 1.Motor 1.Pozisyon.
      {
         myservo.write(180);  
      }
      
      if(t == '4') // 2.Motor 1.Pozisyon.
      {
        
         myservo2.write(180);  
      }
       if(t == '2') // 1.Motor 2.Pozisyon.
      {
         myservo.write(90);  
      }
       if(t == '5') // 2.Motor 2.Pozisyon.
      {
        
         myservo2.write(90);  
      }
       if(t == '3') // 1.Motor 3.Pozisyon.
      {
         myservo.write(30);  
      }
          if(t == '6') // 2.Motor 3.Pozisyon.
      {
        
         myservo2.write(30);  
      }        
}
}
