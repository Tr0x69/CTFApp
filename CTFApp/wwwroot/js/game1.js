let score = 0;
let gameActive = false;
let startTime;

// Function to get the value of a cookie
function getCookie(name) {
    let match = document.cookie.match(new RegExp("(^| )" + name + "=([^;]+)"));
    return match ? match[2] : null;
}

// Function to set a cookie
function setCookie(name, value, days) {
    const d = new Date();
    d.setTime(d.getTime() + days * 24 * 60 * 60 * 1000);
    document.cookie = `${name}=${value};expires=${d.toUTCString()};path=/`;
}

// Initialize the game from the cookie when the page loads
document.addEventListener("DOMContentLoaded", function () {
    // Check if the game was already started from a cookie
    const savedGameState = getCookie("gameState");
    if (savedGameState && savedGameState === "active") {
        gameActive = true;
        score = parseInt(getCookie("score")) || 0;
        startTime = parseInt(getCookie("startTime")) || new Date().getTime();
        document.getElementById("scoreDisplay").innerText = `Score: ${score}`;
        document.getElementById("cookieImage").style.display = "inline-block";
        document.getElementById("startGame").style.display = "none";
    } else {
        document.getElementById("cookieImage").style.display = "none";
        document.getElementById("startGame").style.display = "inline-block";
    }

    // Start the game when the user clicks start
    document.getElementById("startGame").addEventListener("click", function () {
        score = 0;
        gameActive = true;
        document.getElementById("cookieImage").style.display = "inline-block";
        document.getElementById("scoreDisplay").innerText = "Score: 0";
        document.getElementById("startGame").style.display = "none";
        startTime = new Date().getTime();

        // Set cookies to track the game state
        setCookie("gameState", "active", 1);
        setCookie("score", score, 1);
        setCookie("startTime", startTime, 1);
    });

    // Track clicks and score during the game on the cookie image
    document.getElementById("cookieImage").addEventListener("click", function () {
        if (gameActive) {
            score++;
            document.getElementById("scoreDisplay").innerText = "Score: " + score;
           

            // Remove the "crumble" class after the animation ends (resetting it for next click)
            setTimeout(() => {
                this.classList.remove("cookie-crumble");
            }, 500); // match the crumble effect duration

            // Update cookies with the new score
            setCookie("score", score, 1);

            // Game over after 5 seconds
            if (new Date().getTime() - startTime > 5000) {
                gameActive = false;
                
                document.getElementById("cookieImage").style.display = "none";
                document.getElementById("startGame").style.display = "inline-block";

                // Set game state to "inactive" after the game ends
                setCookie("gameState", "inactive", 1);

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
                        alert(`Game Over! You scored ${score} points!`);
                        location.reload(); // Refresh the page to update scores
                    })
                    .catch(error => {
                        console.error("Error:", error);
                        alert("Failed to submit score.");
                    });
            }
        }
    });
});
