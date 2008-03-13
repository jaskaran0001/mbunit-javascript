using System;
using System.Collections.Generic;
using System.Text;

using MbUnit.Core.Framework;
using MbUnit.Core.Runs;

namespace MbUnit.JavaScript {
    public class JavaScriptFixtureAttribute : TestFixturePatternAttribute {
        public JavaScriptFixtureAttribute() : base() {
            this.ApartmentState = System.Threading.ApartmentState.MTA;
        }        
        
        public JavaScriptFixtureAttribute(string description) : base(description) {
            this.ApartmentState = System.Threading.ApartmentState.MTA;
        }

        public override IRun GetRun() {
            return new JavaScriptRun();
        }
    }
}
