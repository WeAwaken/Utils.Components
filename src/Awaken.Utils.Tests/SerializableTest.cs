using Awaken.Utils.Widgets;
using System;
using Xunit;

namespace Awaken.Utils.Tests
{
    public class SerializableTest
    {
        [Fact]
        public void Test()
        {
            // 如果不能调试 注意 是否是 Release模式
            var room = new Room { Id = "1001", RoomNo = "2111", Name = "张三" };

            var value = ObjectSerialize.ToBytesAsync(room).Result;

            var origin = ObjectSerialize.ToObjectAsync<Room>(value).Result;

            Assert.Equal(room.Id, origin.Id);
        }
    }
}
