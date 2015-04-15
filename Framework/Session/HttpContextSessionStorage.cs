using System.Web;

namespace Framework.Session
{
    internal sealed class HttpContextSessionStorage<T> where T : class
    {
        private readonly string _key;
        public HttpContextSessionStorage(string key)
        {
            _key = key;
        }

        public void Save(T state)
        {
            if (state == null)
            {
                Clear();
                return;
            }
            var context = HttpContext.Current;
            if (context == null || context.Session == null)
            {
                return;
            }
            context.Session.Add(_key, state);
        }

        public void Clear()
        {
            var context = HttpContext.Current;
            if (context == null || context.Session == null)
            {
                return;
            }
            context.Session.Remove(_key);
        }

        public T Get()
        {
            var context = HttpContext.Current;
            if (context == null || context.Session == null)
            {
                return default(T);
            }
            return context.Session[_key] as T;
        }
    }
}
