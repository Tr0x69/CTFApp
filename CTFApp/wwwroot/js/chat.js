const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();





function loadMessage() {
	const messages = JSON.parse(localStorage.getItem("chatMessages")) || [];
	const chatMessagesDiv = document.getElementById("chat-messages");
	messages.forEach(msg => {
		const li = document.createElement("li");
		li.innerHTML = msg;
		chatMessagesDiv.appendChild(li);
	})
}

function saveMessage(user, message) {
	const encodedMSG = user + ': ' + message;
	const messages = JSON.parse(localStorage.getItem("chatMessages")) || [];
	messages.push(encodedMSG);
	localStorage.setItem("chatMessages", JSON.stringify(messages));
}



connection.on("ReceiveMessage", function (user, message) {
	const encodedMSG = user + ': ' + message;
	const li = document.createElement("li");

	const range = document.createRange();
	const fragment = range.createContextualFragment(encodedMSG);

	li.appendChild(fragment);
	document.getElementById("chat-messages").appendChild(li);
	saveMessage(user, message);
});

connection.start().then(() => {
	console.log("SignalR Connected");
	loadMessage();
}).catch(function (err) {
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
