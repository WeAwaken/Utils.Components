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
            // ������ܵ��� ע�� �Ƿ��� Releaseģʽ
            var room = new Room { Id = "1001", RoomNo = "2111", Name = "����" };

            var value = ObjectSerialize.ToBytesAsync(room).Result;

            var origin = ObjectSerialize.ToObjectAsync<Room>(value).Result;

            Assert.Equal(room.Id, origin.Id);
        }
    }
}
