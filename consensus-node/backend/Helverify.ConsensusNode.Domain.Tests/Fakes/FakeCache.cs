using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace Helverify.ConsensusNode.Domain.Tests.Fakes
{
    internal class FakeCache: IMemoryCache
    {
        private object _obj;

        public FakeCache(object obj)
        {
            _obj = obj;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(object key, out object value)
        {
            value = _obj;

            return true;
        }

        public ICacheEntry CreateEntry(object key)
        {
            return new Mock<ICacheEntry>().Object;
        }

        public void Remove(object key)
        {
            throw new NotImplementedException();
        }
    }
}
