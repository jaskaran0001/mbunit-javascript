using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript.Engines.Microsoft.Interop {
    internal enum tagSCRIPTTEXT {
        SCRIPTTEXT_DELAYEXECUTION      = 0x00000001,
        SCRIPTTEXT_ISVISIBLE           = 0x00000002,
        SCRIPTTEXT_ISEXPRESSION        = 0x00000020,
        SCRIPTTEXT_ISPERSISTENT        = 0x00000040,
        SCRIPTTEXT_HOSTMANAGESSOURCE   = 0x00000080,
        SCRIPTTEXT_ALL_FLAGS           = SCRIPTTEXT_DELAYEXECUTION 
                                       | SCRIPTTEXT_ISVISIBLE 
                                       | SCRIPTTEXT_ISPERSISTENT 
                                       | SCRIPTTEXT_HOSTMANAGESSOURCE
    }
}
