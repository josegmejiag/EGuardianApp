using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EGuardian.Helpers.Eventos
{
    public class EventosAgrupacion<K, T> : ObservableCollection<T>
    {
        public K Key { get; private set;}

        public EventosAgrupacion(K key, IEnumerable<T> eventos)
        {
            Key = key;
            foreach (var evento in eventos)
                this.Items.Add(evento);
        }
    }
}
