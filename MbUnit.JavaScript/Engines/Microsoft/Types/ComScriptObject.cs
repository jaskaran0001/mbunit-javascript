﻿/* 
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
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.Expando;

using MbUnit.JavaScript.Engines.Base;

namespace MbUnit.JavaScript.Engines.Microsoft.Types {
    internal class ComScriptObject : ScriptObjectBase, IComObjectWrapper {
        private readonly IExpando innerObject;
        private readonly ComScriptConverter converter;
        private readonly IComScriptInvoker invoker;

        public ComScriptObject(IExpando innerObject, ComScriptConverter converter, IComScriptInvoker invoker) {
            this.innerObject = innerObject;
            this.converter = converter;
            this.invoker = invoker;
        }

        public override void Add(string key, object value) {
            var property = this.innerObject.AddProperty(key);
            this.SetValue(property, value);
        }

        public override bool ContainsKey(string key) {
            return this.GetProperty(key) != null;
        }

        public override bool Remove(string key) {
            var property = this.GetProperty(key);
            if (property == null)
                return false;

            this.innerObject.RemoveMember(property);
            return true;
        }

        public override bool TryGetValue(string key, out object value) {
            var property = this.GetProperty(key);
            bool found = (property != null);

            value = found ? this.GetValue(property) : null;
            return found;
        }

        public override ICollection<string> Keys {
            get { throw new NotImplementedException(); }
        }

        public override ICollection<object> Values {
            get { throw new NotImplementedException(); }
        }

        public override object this[string key] {
            get {
                var property = this.GetProperty(key, true);
                return this.GetValue(property);
            }
            set {
                var property = this.GetProperty(key, false);
                if (property != null) {
                    this.SetValue(property, value);
                }
                else {
                    this.Add(key, value);
                }
            }
        }

        public override void Clear() {
            var members = this.GetProperties();
            foreach (var member in members) {
                this.innerObject.RemoveMember(member);
            }
        }

        public override int Count {
            get { return this.GetProperties().Length; }
        }

        public override IEnumerator<KeyValuePair<string, object>> GetEnumerator() {
            foreach (var property in this.GetProperties()) {
                yield return new KeyValuePair<string, object>(
                    property.Name, this.GetValue(property)
                );
            }
        }

        public override object Invoke(string key, params object[] args) {
            return this.invoker.Invoke(this.innerObject, key, args);
        }

        private PropertyInfo GetProperty(string name) {
            return this.GetProperty(name, false);
        }

        private PropertyInfo GetProperty(string name, bool throwIfNotFound) {
            var property = this.innerObject.GetProperty(name, BindingFlags.Instance);
            if (throwIfNotFound && property == null)
                throw new KeyNotFoundException();

            return property;
        }

        private object GetValue(PropertyInfo property) {
            var value = property.GetValue(this.innerObject, null);
            return this.converter.ConvertFromScript(value);
        }

        private void SetValue(PropertyInfo property, object value) {
            value = this.converter.ConvertToScript(value);
            property.SetValue(this.innerObject, value, null);
        }

        private PropertyInfo[] GetProperties() {
            return this.innerObject.GetProperties(BindingFlags.Instance);
        }

        internal IExpando InnerObject {
            get { return this.innerObject; }
        }

        #region IComObjectWrapper Members

        object IComObjectWrapper.ComObject {
            get { return this.InnerObject; }
        }

        #endregion
    }
}
