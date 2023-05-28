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
                $("<div>").addClass("chat-hour").text(formatTime(new Date()) + " ").append(
                    $("<span>").addClass("fa fa-check-circle")
                ),
                $("<div>").addClass("chat-text").text(message),
                $("<div>").addClass("chat-avatar").append(
                    $("<img>").attr("src", "https://www.bootdey.com/img/Content/avatar/avatar3.png").attr("alt", "Retail Admin"),
                    $("<div>").addClass("chat-name").text(senderUsername)
                )
            );

            // Append the message to the chat box
            $(".chat-box").append(messageItem);

            // Scroll to the bottom of the chat box
            $(".chatContainerScroll").scrollTop($(".chatContainerScroll")[0].scrollHeight);
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

            if (selectedUsername) {
                $("#messageInput").prop("disabled", false);
                $("#sendMessageBtn").prop("disabled", false);
                $(".chat-container").removeClass("disabled");
            } else {
                $("#messageInput").prop("disabled", true);
                $("#sendMessageBtn").prop("disabled", true);
                $(".chat-container").addClass("disabled");
            }
        });

        $(document).on("click", ".chat-container.disabled", function (e) {
            e.preventDefault();
            return false;
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
                                $("<div>").addClass("chat-hour").text(formatTime(new Date()) + " ").append(
                                    $("<span>").addClass("fa fa-check-circle")
                                ),
                                $("<div>").addClass("chat-text").text(message),
                                $("<div>").addClass("chat-avatar").append(
                                    $("<img>").attr("src", "https://www.bootdey.com/img/Content/avatar/avatar3.png").attr("alt", "Retail Admin"),
                                    $("<div>").addClass("chat-name").text(recipientUsername)
                                )
                            );
                            $(".chat-box").append(messageItem);

                            // Scroll to the bottom of the chat box
                            $(".chatContainerScroll").scrollTop($(".chatContainerScroll")[0].scrollHeight);
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
    });

