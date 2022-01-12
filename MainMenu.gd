extends Control


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


# Called when the node enters the scene tree for the first time.
func _ready():
	GameManager.connect("connected_players_update", self, "_on_players_update")


func _on_players_update(players):
	#for player in players:
	#	#$GameLobby/PlayerList.add_item("%s:%s"%[str(player),str(players[player])])
	$GameLobby/PlayerList.text = str(players)
	$GameLobby/StartGameButton.disabled = players.size() < 2

func _on_JoinMenuButton_pressed():
	$MainMenu.hide()
	$JoinServerMenu.show()

func _on_BackButton_pressed():
	$JoinServerMenu.hide()
	$MainMenu.show()

func _on_JoinButton_pressed():
	GameManager.join_server($JoinServerMenu/HBoxContainer/AddressInput.text)
	$JoinServerMenu.hide()
	$GameLobby.show()
	$GameLobby/StartGameButton.hide()

func _on_HostButton_pressed():
	GameManager.host_game()
	$MainMenu.hide()
	$GameLobby.show()
	$GameLobby/StartGameButton.disabled = true
	$GameLobby/StartGameButton.show()

func _on_StopHostingButton_pressed():
	GameManager.close_connections()
	#$GameLobby/PlayerList.clear()
	$GameLobby/PlayerList.text=""
	$GameLobby.hide()
	$MainMenu.show()


func _on_StartGameButton_pressed():
	$GameLobby.hide()
	GameManager.start_game()
