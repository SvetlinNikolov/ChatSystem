$(document).ready(function () {
    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub")
        .build();

    // Receive message event handler
    connection.on("ReceiveMessage", function (message, senderUsername) {
        var selectedUsername = $(".selected-user .name").text();

        // Check if the received message is intended for the selected user
        var isSender = senderUsername === selectedUsername;

        var messageItem = $("<li>").addClass(isSender ? "chat-right" : "chat-left").append(
            $("<div>").addClass("chat-hour").text(formatTime(new Date())),
            $("<div>").addClass("chat-text").text(message),
            $("<div>").addClass("chat-avatar").append(
                $("<img>").attr("src", "https://w7.pngwing.com/pngs/754/2/png-transparent-samsung-galaxy-a8-a8-user-login-telephone-avatar-pawn-blue-angle-sphere-thumbnail.png").attr("alt", "Retail Admin"),
                $("<div>").addClass("chat-name").text(senderUsername)
            )
        );

        // Get the chat box element
        var chatBox = $(".chat-box");

        // Check if there are existing messages in the chat box
        if (chatBox.children().length > 0) {
            // Insert the new message after the last existing message
            chatBox.children().last().after(messageItem);
        } else {
            // Append the new message to the chat box if it's the first message
            chatBox.append(messageItem);
        }
    });

    // Start the connection
    connection.start()
        .then(function () {
            console.log("Connected to the chat hub");
        })
        .catch(function (error) {
            console.error("Error connecting to the chat hub:", error);
        });

    $(document).on("click", ".person", function () {
        console.log("Clicked on person");
        $(".person").removeClass("selected-user");
        $(this).addClass("selected-user");
        var selectedUsername = $(this).find(".name").text();
        var selectedUserId = $(this).data("chat");
        $(".selected-user .name").text(selectedUsername);
        console.log("Selected user: " + selectedUsername + ", ID: " + selectedUserId);

        // Clear the chat box
        $(".chat-box").html('');

        if (selectedUsername) {
            $("#messageInput").prop("disabled", false);
            $("#sendMessageBtn").prop("disabled", false);
            $(".chat-container").removeClass("disabled");

            // Reset skip and take values
            skip = 0;
            take = 5;

            // Get initial messages for the selected user
            getMessages(selectedUserId, skip, take);
        } else {
            $("#messageInput").prop("disabled", true);
            $("#sendMessageBtn").prop("disabled", true);
            $(".chat-container").addClass("disabled");
        }
    });

    $("#sendMessageBtn").click(function () {
        var recipientId = $(".selected-user").data("chat"); // Get the recipient's ID
        var message = $("#messageInput").val().trim();
        if (message !== "") {
            // Check if the connection is in the 'Connected' state before sending the message
            if (connection.state === signalR.HubConnectionState.Connected) {
                // Send the message to the hub
                connection.invoke("SendMessage", recipientId, message)
                    .then(function () {
                        // Clear the input field after sending the message
                        $("#messageInput").val("");

                        // Create a message item for the sender and append it to the chat box
                        var recipientUsername = $(".selected-user .name").text(); // Get the recipient's username
                        var messageItem = $("<li>").addClass("chat-right").append(
                            $("<div>").addClass("chat-hour").text(formatTime(new Date())),
                            $("<div>").addClass("chat-text").text(message),
                            $("<div>").addClass("chat-avatar").append(
                                $("<img>").attr("src", "https://w7.pngwing.com/pngs/754/2/png-transparent-samsung-galaxy-a8-a8-user-login-telephone-avatar-pawn-blue-angle-sphere-thumbnail.png").attr("alt", "Retail Admin"),
                                $("<div>").addClass("chat-name").text(recipientUsername)
                            )
                        );
                        $(".chat-box").append(messageItem);
                    })
                    .catch(function (error) {
                        console.error("Error sending message:", error);
                    });
            } else {
                console.error("Cannot send message. Connection is not in the 'Connected' state.");
            }
        }
    });

    // Helper function to format time as HH:mm
    function formatTime(date) {
        var hours = date.getHours();
        var minutes = date.getMinutes();
        return (hours < 10 ? "0" + hours : hours) + ":" + (minutes < 10 ? "0" + minutes : minutes);
    }

    // Function to retrieve messages for a user with skip and take parameters
    function getMessages(userId, skip, take) {
        $.ajax({
            url: "/Chat/GetMessages",
            data: { userId: userId, skip: skip, take: take },
            method: "GET",
            success: function (data) {
                // Add the retrieved messages to the chat box
                data.forEach(function (message) {
                    var messageItem = $("<li>").addClass("chat-left").append(
                        $("<div>").addClass("chat-hour").text(formatTime(new Date(message.timestamp))),
                        $("<div>").addClass("chat-text").text(message.content),
                        $("<div>").addClass("chat-avatar").append(
                            $("<img>").attr("src", "https://w7.pngwing.com/pngs/754/2/png-transparent-samsung-galaxy-a8-a8-user-login-telephone-avatar-pawn-blue-angle-sphere-thumbnail.png").attr("alt", "Retail Admin"),
                            $("<div>").addClass("chat-name").text(message.senderUsername)
                        )
                    );

                    // Prepend the message to the chat box
                    $(".chat-box").prepend(messageItem);
                });
            },
            error: function (error) {
                console.error("Error getting messages:", error);
            }
        });
    }

    // Load older messages when the button is clicked
    $(document).on("click", "#loadOlderMessagesBtn", function () {
        var selectedUserId = $(".selected-user").data("chat");

        if (selectedUserId) {
            // Get the current number of messages in the chat box
            var currentMessageCount = $(".chat-box > li").length;

            // Increase skip value to load older messages
            skip = currentMessageCount;
            take = 5;

            // Get older messages for the selected user
            getMessages(selectedUserId, skip, take);
        }
    });
});
