extends Control


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


# Called when the node enters the scene tree for the first time.
func _ready():
	Lobby.connect("connected_players_update", self, "_on_players_update")
	Lobby.connect("disconnected_from_host", self, "_on_host_disconnect")


func _on_players_update(players):
	$GameLobby/PlayerList.clear()
	for player_id in players:
		$GameLobby/PlayerList.add_item(players[player_id])
	$GameLobby/StartGameButton.disabled = players.size() < 2

func _on_host_disconnect():
	$GameLobby/PlayerList.clear()
	$GameLobby.hide()
	$MainMenu.show()
	

func _on_JoinMenuButton_pressed():
	$MainMenu.hide()
	$JoinServerMenu.show()

func _on_HostMenuButton_pressed():
	$MainMenu.hide()
	$HostServerMenu.show()

func _on_BackButton_pressed():
	$HostServerMenu.hide()
	$JoinServerMenu.hide()
	$MainMenu.show()

func _on_JoinButton_pressed():
	Lobby.join_server($JoinServerMenu/ServerAddress/Input.text, $JoinServerMenu/PlayerName/Input.text)
	$JoinServerMenu.hide()
	$GameLobby.show()
	$GameLobby/StartGameButton.hide()
	$GameLobby/DisconnectButton.text = "Disconnect"

func _on_HostButton_pressed():
	Lobby.host_game($HostServerMenu/PlayerName/Input.text)
	$HostServerMenu.hide()
	$GameLobby.show()
	$GameLobby/StartGameButton.disabled = true
	$GameLobby/StartGameButton.show()
	$GameLobby/DisconnectButton.text = "Stop Hosting"

func _on_StopHostingButton_pressed():
	Lobby.close_connections()
	$GameLobby/PlayerList.clear()
	$GameLobby.hide()
	$MainMenu.show()

func _on_StartGameButton_pressed():
	$GameLobby.hide()
	# TODO: get order of player list, use as seating/dealer order, add shuffle/reorder buttons
	Lobby.start_game()


func _on_QuitButton_pressed():
	get_tree().quit()
