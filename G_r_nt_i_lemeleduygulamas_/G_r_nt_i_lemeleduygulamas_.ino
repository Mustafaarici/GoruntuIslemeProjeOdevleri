
int led1 = 2; // led pinleri.
int led2 = 3;
int led3 = 4;
int led4 = 5;
int led5 = 6;
int led6 = 7;
int led7 = 8;
int led8 = 9;
int led9 = 10;
char a; // Seriporttan gelen verinin aktarıldığı değişken.

void setup() {
  pinMode(led1, OUTPUT); 
  pinMode(led2, OUTPUT); 
  pinMode(led3, OUTPUT); 
  pinMode(led4, OUTPUT); 
  pinMode(led5, OUTPUT); 
  pinMode(led6, OUTPUT); 
  pinMode(led7, OUTPUT); 
  pinMode(led8, OUTPUT); 
  pinMode(led9, OUTPUT);  
  Serial.begin(9600);
}

void loop() {

        digitalWrite(led1, LOW); // İlk durumda tüm ledler sönük durumda olarak ayarlandı.
        digitalWrite(led2, LOW);
        digitalWrite(led3, LOW);
        digitalWrite(led6, LOW);
        digitalWrite(led5, LOW);
        digitalWrite(led7, LOW);
        digitalWrite(led4, LOW);
        digitalWrite(led8, LOW);
        digitalWrite(led9, LOW);
       
if(Serial.available()) 
  {
    
     a = Serial.read(); // Seriport okuma işlemi.
    if(a == 'a')
    {
        digitalWrite(led1, HIGH);   
        delay(50);
      }
       if(a == 'c')
    {
        digitalWrite(led2, HIGH);   
          delay(50);
      }
      if(a == 'd')
    {
        digitalWrite(led3, HIGH);   
     delay(50);
      }
        if(a == 'e')
    {
        digitalWrite(led6, HIGH);   
     delay(50);
      }
       if(a == 'f')
    {
        digitalWrite(led5, HIGH);  
     delay(50);
      }
       if(a == 'g')
    {
        digitalWrite(led4, HIGH);   
     delay(50);
      }

if(a == 'h')
    {
        digitalWrite(led7, HIGH);  
     delay(50);
      }
if(a == 'i')
    {
        digitalWrite(led8, HIGH);   
     delay(50);
      }
if(a == 'l')
    {
        digitalWrite(led9, HIGH);   
     delay(50);
      }
        if(a == 'b') //Nesne algılanmadığında tüm ledler söner.
      {
        digitalWrite(led1, LOW);
        digitalWrite(led2, LOW);
        digitalWrite(led3, LOW);
        digitalWrite(led6, LOW);
        digitalWrite(led5, LOW);
        digitalWrite(led7, LOW);
        digitalWrite(led8, LOW);
        digitalWrite(led9, LOW);
 
        }

           
        
           
           

  }
  
}

