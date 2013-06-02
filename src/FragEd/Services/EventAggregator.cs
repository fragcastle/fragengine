using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FragEd.Services
{
    public class EventCollection : List<Action<Object, EventArgs>>{}

    public class EventAggregator
    {

        public static readonly EventAggregator Current = new EventAggregator();

        private Dictionary<string, EventCollection> _events;

        private EventAggregator()
        {
            _events = new Dictionary<string, EventCollection>();
        }

        public EventCollection On( string index )
        {
            EventCollection collection = _events.ContainsKey( index ) ? _events[ index ] : new EventCollection();
            if( !_events.ContainsKey(index) )
            {
                _events.Add(index, collection);
            }
            return collection;
        }

        public void Trigger( string index, object sender = null, EventArgs args = null )
        {
            var collection = _events[ index ];
            foreach(var action in collection)
            {
                action( sender, args ?? EventArgs.Empty );
            }
        }
    }
}
