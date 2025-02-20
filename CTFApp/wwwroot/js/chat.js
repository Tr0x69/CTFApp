const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
connection.on("ReceiveMessage", function (user, message) {
	const encodedMSG = user + ': ' + message;
	const li = document.createElement("li");

	li.innerHTML = encodedMSG;
	document.getElementById("chat-messages").appendChild(li);
});

connection.start().catch(function (err) {
	return console.error(err.toString());
})

document.getElementById("sendButton").addEventListener("click", function (event) {

	const user = document.getElementById("userName").value;
	const message = document.getElementById("messageInput").value;
	connection.invoke("SendMessage", user, message).catch(function (err) {
		return console.error(err.toString());
	});
	document.getElementById("messageInput").value = "";
	event.preventDefault();
});