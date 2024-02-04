using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Everest {
    [Serializable]
    public class EnumResource<TEnum, TItem> : IEnumerable<KeyValuePair<TEnum, TItem>> where TEnum : Enum {
        [SerializeField] TItem[] items;

        public TEnum[] Keys => (TEnum[])Enum.GetValues(typeof(TEnum));
        public TItem[] Values => items;
        public int Count => items.Length;

        public TItem this[TEnum x] => Get(x);

        public TItem Get(TEnum enumValue) {
            //Không đổi trực tiêp index = (int)enumValue để không bị lỗi với enum được gán giá trị
            int index = Array.IndexOf(Enum.GetValues(typeof(TEnum)), enumValue);
            return items[index];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<KeyValuePair<TEnum, TItem>> GetEnumerator() {
            var keys = Keys;
            for (int i = 0; i < keys.Length; i++) {
                yield return new KeyValuePair<TEnum, TItem>(keys[i], items[i]);
            }
        }
    }
}