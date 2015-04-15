using System.Web;
using System.Web.SessionState;

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
            var context = HttpContext.Current.Session;
            if (context == null)
            {
                return;
            }
            context.Add(_key, state);
        }

        public void Clear()
        {
            var context = HttpContext.Current.Session;
            if (context == null)
            {
                return;
            }
            context.Remove(_key);
        }

        public T Get()
        {
            var context = HttpContext.Current.Session;
            if (context == null)
            {
                return default(T);
            }
            return context[_key] as T;
        }
    }
}
