using System;
using System.Collections.Generic;
using System.Text;

using MbUnit.Core.Framework;
using MbUnit.Core.Runs;
using MbUnit.JavaScript.References;

namespace MbUnit.JavaScript {
    public class JavaScriptFixtureAttribute : TestFixturePatternAttribute {
        private static readonly Type DefaultReferenceExtractorType = typeof(JavaScriptXmlReferenceExtractor);
        
        private Type referenceExtractorType;

        public JavaScriptFixtureAttribute() : base() {
            this.ApartmentState = System.Threading.ApartmentState.MTA;
        }        
        
        public JavaScriptFixtureAttribute(string description) : base(description) {
            this.ApartmentState = System.Threading.ApartmentState.MTA;
        }

        public Type ReferenceExtractorType {
            get { return referenceExtractorType ?? DefaultReferenceExtractorType; }
            set { referenceExtractorType = value; }
        }

        public override IRun GetRun() {
            var referenceExtractor = (IJavaScriptReferenceExtractor)Activator.CreateInstance(this.ReferenceExtractorType);
            return new JavaScriptRun(referenceExtractor);
        }
    }
}
