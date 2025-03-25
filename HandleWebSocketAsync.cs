using System.Net.WebSockets;
using System.Text;

namespace WebSocketExample
{
    public class WebSocketHandler
    {
        public async Task HandleWebSocketAsync(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                // Read client message
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Received: {message}");

                // Send a response back to the client
                var responseMessage = $"Server: Received '{message}' at {DateTime.Now}";
                var responseBytes = Encoding.UTF8.GetBytes(responseMessage);
                await webSocket.SendAsync(new ArraySegment<byte>(responseBytes), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            // Close WebSocket connection
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
