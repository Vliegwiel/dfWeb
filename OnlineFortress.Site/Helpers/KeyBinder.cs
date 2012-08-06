using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineFortress.Site.Helpers {
    public class KeyBinder {


        const int maxCommandLenght = 4;

        const byte ESC = 27;
        
        const byte CtrlLowerCaseOffset = 96;
        const byte CtrlUpperCaseOffset = 64;

        public static byte[] ParsKeypress(byte character, bool hasAlt, bool hasCtrl, bool hasShift) {

            byte[] ret = new byte[maxCommandLenght];
            int i = 0;


            if (hasAlt) {
                ret[i] = ESC;
                i++;
            }

            switch (character) {
                case 255:
                    // Needed for up down keys etc
                    break;
            }

            // control sends bytes 0 - 26 which will overwrite the normal char
            if (hasCtrl) {
                if ((int)character > CtrlLowerCaseOffset) {
                    ret[i] = (byte)((int)character - (int)CtrlLowerCaseOffset);
                    i++;
                } else {
                    ret[i] = (byte)((int)character - (int)CtrlUpperCaseOffset);
                    i++;
                }                
                return ret;
            }

            ret[i] = character;

            return ret;
        }   
    }
}
