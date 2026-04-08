using Cysharp.Threading.Tasks;

namespace Test
{
    public class DependencyTest
    {
        public async UniTask Test()
        {
            await UniTask.Delay(100);
        }
    }
}