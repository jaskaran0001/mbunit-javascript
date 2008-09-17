/* 
  Copyright (c) 2008-2009, Andrey Shchekin
  All rights reserved.
 
  Redistribution and use in source and binary forms, with or without modification, are permitted provided
  that the following conditions are met:
    * Redistributions of source code must retain the above copyright notice, this list of conditions
      and the following disclaimer.
    
    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions
      and the following disclaimer in the documentation and/or other materials provided with the
      distribution.
 
    * Neither the name of the Andrey Shchekin nor the names of his contributors may be used to endorse or 
      promote products derived from this software without specific prior written permission.

  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
  WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
  PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
  ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
  TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
  HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
  POSSIBILITY OF SUCH DAMAGE.
*/

using System;

using MbUnit.Core.Framework;
using MbUnit.Core.Runs;
using MbUnit.JavaScript.Engines;
using MbUnit.JavaScript.Engines.Microsoft;
using MbUnit.JavaScript.References;

namespace MbUnit.JavaScript {
    public class ScriptFixtureAttribute : TestFixturePatternAttribute {
        private static readonly Type DefaultReferenceExtractorType = typeof(ScriptXmlReferenceExtractor);
        private static readonly Type DefaultScriptEngineType = typeof(ComActiveScriptEngine);
        
        private Type referenceExtractorType;
        private Type scriptEngineType;

        public ScriptFixtureAttribute() {
            this.ApartmentState = System.Threading.ApartmentState.STA;
        }        
        
        public ScriptFixtureAttribute(string description) : base(description) {
            this.ApartmentState = System.Threading.ApartmentState.STA;
        }

        public Type ScriptEngineType {
            get { return scriptEngineType ?? DefaultScriptEngineType; }
            set { scriptEngineType = value; }
        }

        public Type ReferenceExtractorType {
            get { return referenceExtractorType ?? DefaultReferenceExtractorType; }
            set { referenceExtractorType = value; }
        }

        public override IRun GetRun() {
            var engine = (IScriptEngine)Activator.CreateInstance(this.ScriptEngineType);
            var referenceExtractor = (IScriptReferenceExtractor)Activator.CreateInstance(this.ReferenceExtractorType);
            
            return new ScriptRun(engine, referenceExtractor);
        }
    }
}
