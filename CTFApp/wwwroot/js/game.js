let score = 0;
let gameActive = false;
let startTime;

document.getElementById("startGame").addEventListener("click", function () {
    score = 0;
    gameActive = true;
    document.getElementById("clickButton").style.display = "inline-block";
    document.getElementById("scoreDisplay").innerText = "Score: 0";
    document.getElementById("startGame").style.display = "none";
    startTime = new Date().getTime();
});

document.getElementById("clickButton").addEventListener("click", function () {
    if (gameActive) {
        score++;
        document.getElementById("scoreDisplay").innerText = "Score: " + score;

        if (new Date().getTime() - startTime > 5000) {
            gameActive = false;
            alert(`Game Over! You scored ${score} points!`);
            document.getElementById("clickButton").style.display = "none";
            document.getElementById("startGame").style.display = "inline-block";

            const userData = {
                UserName: document.getElementById("userName").value,
                UserScore: score,
            };

            fetch("/api/game/submitscore", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(userData)
            })
                .then(response => response.json())
                .then(data => {
                    alert(data.message);
                    location.reload(); // Refresh the page to update scores
                })
                .catch(error => {
                    console.error("Error:", error);
                    alert("Failed to submit score.");
                });
        }
    }
});