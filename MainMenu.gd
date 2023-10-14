extends Control

var ready_check = load("res://assets/art/check-mark.svg")
var not_ready = load("res://assets/art/purple-square.svg")

func _ready():
	Lobby.connected_players_update.connect(_on_players_update)
	Lobby.disconnected_from_host.connect(_on_host_disconnect)
	print(OS.get_cmdline_args())
	var run_as_server = OS.has_feature("server") or "--server" in OS.get_cmdline_args()
	# run_as_server = true # uncomment to force server mode, for testing through the godot UI
	if run_as_server:
		print("lets be a server")
		hide()
		Lobby.host_server()
	else:
		print("lets be a client")
		$MainMenu.show()
		$JoinServerMenu.hide()
		$GameLobby.hide()

func _on_players_update(players):
	$GameLobby/PlayerList.clear()
	for player_id in players:
		var status_icon = (ready_check if players[player_id]["ready"] else not_ready)
		$GameLobby/PlayerList.add_item(players[player_id]["name"], status_icon)
	$GameLobby/ReadyButton.disabled = players.size() < 2

func _on_host_disconnect():
	$GameLobby/PlayerList.clear()
	$GameLobby.hide()
	$MainMenu.show()

func _on_JoinMenuButton_pressed():
	$MainMenu.hide()
	$JoinServerMenu.show()

func _on_BackButton_pressed():
	$JoinServerMenu.hide()
	$MainMenu.show()

func _on_JoinButton_pressed():
	Lobby.join_server($JoinServerMenu/ServerAddress/Input.text, $JoinServerMenu/PlayerName/Input.text)
	$JoinServerMenu.hide()
	$GameLobby.show()
	$GameLobby/ReadyButton.text = "Ready"
	$GameLobby/DisconnectButton.text = "Disconnect"

func _on_StopHostingButton_pressed():
	Lobby.close_connections()
	$GameLobby/PlayerList.clear()
	$GameLobby.hide()
	$MainMenu.show()

func _on_ReadyButton_pressed():
	Lobby.toggle_ready.rpc()

func _on_game_start():
	pass


func _on_QuitButton_pressed():
	get_tree().quit()



func _on_UpButton_pressed():
	var idx = $GameLobby/PlayerList.get_selected_items()[0]
	$GameLobby/PlayerList.move_item(idx,idx-1)
	_on_PlayerList_item_selected(idx-1)

func _on_DownButton_pressed():
	var idx = $GameLobby/PlayerList.get_selected_items()[0]
	$GameLobby/PlayerList.move_item(idx,idx+1)
	_on_PlayerList_item_selected(idx+1)

func _on_RandomButton_pressed():
	#$GameLobby/PlayerList.unselect_all()
	for idx in range($GameLobby/PlayerList.get_item_count()):
		var rand_idx = randi_range(idx,$GameLobby/PlayerList.get_item_count()-1)
		$GameLobby/PlayerList.move_item(idx, rand_idx)


func _on_PlayerList_nothing_selected(_at_position, _mouse_button_index):
	$GameLobby/SeatingControls/DownButton.disabled = true
	$GameLobby/SeatingControls/UpButton.disabled = true


func _on_PlayerList_item_selected(index):
	$GameLobby/SeatingControls/DownButton.disabled = (index == ($GameLobby/PlayerList.get_item_count() - 1))
	$GameLobby/SeatingControls/UpButton.disabled = (index == 0)


func _on_AddCPUButton_pressed():
	Lobby.new_cpu()


func _on_RemoveCPUButton_pressed():
	Lobby.rem_cpu()
