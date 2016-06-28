PRODUCT_ID(551); //Drone Fishing Kit
PRODUCT_VERSION(1);

// This #include statement was automatically added by the Particle IDE.
#include "Tinker-Servo/Tinker-Servo.h"

//
// COMPLEMENTARY API CALL
// POST /v1/devices/{DEVICE_ID}/{FUNCTION}
//
// # EXAMPLE REQUEST
// curl https://api.spark.io/v1/devices/0123456789abcdef01234567/servo \
//     -d access_token=1234123412341234123412341234123412341234 \
//     -d "args=180"
//----------------------------------------------------------------------------

Servo myservo;  // create servo object to control a servo
                // a maximum of eight servo objects can be created

const int buttonPin = D1;     // the number of the pushbutton pin
const int servoPin = D0;     // the number of the servo pin
const int ledPin = D7;     // the number of the LED pin

// variables will change:
int buttonState = 0;         // variable for reading the pushbutton status
int pos = 0;    // variable to store the servo position
bool remoteOverride = false;  // variable to tell the loop the command is being overridden remotely.

  STARTUP(WiFi.selectAntenna(ANT_INTERNAL)); // selects the CHIP antenna
  //STARTUP(WiFi.selectAntenna(ANT_EXTERNAL)); // selects the u.FL antenna
  
void setup()
{
  // attaches the servo on the A0 pin to the servo object
  myservo.attach(servoPin);
  
  pinMode(buttonPin, INPUT);
  pinMode(ledPin, OUTPUT);
  
  Particle.publish("Setup complete", buttonState);
  // turn on 20k pullup resistor
  //digitalWrite(buttonPin, HIGH);
  
  // register the Spark function
  Particle.function("servo", updateServo);
}

void loop()
{
  // do nothing
  // read the state of the pushbutton value:
  buttonState = digitalRead(buttonPin);

    if (!remoteOverride)
    {
        
      Particle.publish("Button state", buttonState);
      // check if the pushbutton is pressed.
      // if it is, the buttonState is HIGH:
      if (buttonState == HIGH) {
          
          if (pos != 150)
          {
                Particle.publish("Button on , moving servo", buttonState);
                pos = 150;
                myservo.write(pos);
                delay(1000);
          }
      } else {
        // turn LED off:
            if (pos != 30)
            {
                Particle.publish("Button off , resetting servo", pos);
                pos = 30;
                myservo.write(pos);
                delay(1000);
            }
      }
    }
}

//this function automagically gets called upon a matching POST request
int updateServo(String command)
{
    remoteOverride = true;
    
    // convert string to integer
    uint8_t pos = command.toInt();

      // process if integer is 0 - 180
      if(pos <= 180)
      {
        // tell servo to go to position in variable 'pos'
        myservo.write(pos);
    
        if(pos == 0) {remoteOverride= false;}
        
        // return an integer success code that can be processed by our app
        return 200;
      }
      else {
        // return an integer error code that can be processed by our app
        return -1;
      }
      
      
}

