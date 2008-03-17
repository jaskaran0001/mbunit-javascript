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
