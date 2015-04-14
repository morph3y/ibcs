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
            HttpContext context = HttpContext.Current;
            if (context == null)
            {
                return;
            }
            context.Items[_key] = state;
        }

        public void Clear()
        {
            HttpContext context = HttpContext.Current;
            if (context == null)
            {
                return;
            }
            context.Items.Remove(_key);
        }

        public T Get()
        {
            HttpContext context = HttpContext.Current;
            if (context == null)
            {
                return default(T);
            }
            return context.Items[_key] as T;
        }
    }
}
