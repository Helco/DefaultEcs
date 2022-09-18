using NFluent;
using Xunit;

namespace DefaultEcs.Test
{
    public sealed class ComponentsTest
    {
        #region Tests

        [Fact]
        public void GetComponents_Should_return_fast_access_to_component()
        {
            using World world = new(4);

            Entity entity1 = world.CreateEntity();
            Entity entity2 = world.CreateEntity();
            Entity entity3 = world.CreateEntity();
            Entity entity4 = world.CreateEntity();

            entity1.Set("1");
            entity2.Set("2");
            entity3.Set("3");
            entity4.Set("4");

            Components<string> strings = world.GetComponents<string>();

            Check.That(entity1.Get<string>()).IsEqualTo(strings[entity1]);
            Check.That(entity2.Get<string>()).IsEqualTo(strings[entity2]);
            Check.That(entity3.Get<string>()).IsEqualTo(strings[entity3]);
            Check.That(entity4.Get<string>()).IsEqualTo(strings[entity4]);
        }

#if SAFEDEBUG
        [Fact]
        public void Safe_GetComponents_Should_throw_on_reallocation()
        {
            using World world = new();

            var entity = world.CreateEntity();
            entity.Set(1);

            Check.ThatCode(() =>
            {
                var ints = world.GetComponents<int>();
                // Cause a reallocation
                for (int i = 0; i < 8; i++)
                {
                    world.CreateEntity().Set(i);
                }
                return ints[entity];
            }).Throws<DefaultEcsException>();
        }
#endif

        #endregion
    }
}
