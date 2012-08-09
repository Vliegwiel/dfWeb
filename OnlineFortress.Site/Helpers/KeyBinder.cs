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

        public static byte[] ParsKeypress(byte character, byte keycode, bool hasAlt, bool hasCtrl, bool hasShift) {

            byte[] ret = new byte[maxCommandLenght];
            int i = 0;

            switch (character) {
                case 0:
                    switch (keycode) {
                        case 33:
                            // Page Up esc[F
                            //return new byte[] { 27, 91 };
                        case 34:
                            // Page Down esc[F
                            //return new byte[] { 27, 91 };
                        case 35:
                            // End esc[F
                            return new byte[] { 27, 91, 70 };
                        case 36:
                            // Home esc[H
                            return new byte[] { 27, 91, 71 };
                        case 37:
                            // Arrow Left esc[D
                            return new byte[] { 27, 91, 68 };
                        case 38:
                            // Arrow Up  esc[A
                            return new byte[] { 27, 91, 65 };
                        case 39:
                            // Arrow Right esc[C
                            return new byte[] { 27, 91, 67 };
                        case 40:
                            // Arrow Down esc[B
                            return new byte[] { 27, 91, 66};
                        case 46:
                            // Delete
                            return new byte[] { 27, 91 };

                    }
                    // Needed for up down keys etc
                    break;
                case 8:
                    return new byte[] { 127 };
            }

            if (hasAlt) {
                ret[i] = ESC;
                i++;
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
