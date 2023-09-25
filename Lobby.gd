extends Node

signal connected_players_update(players)
signal disconnected_from_host

const GAME_PORT = 20202

var GameScene : PackedScene = load("res://Game.tscn")

var i_am_host = false
var my_name
var players = {}
var game

# server code

func host_game(player_name):
	my_name = player_name
	players[1] = my_name
	connected_players_update.emit(players)
	var peer = ENetMultiplayerPeer.new()
	peer.create_server(GAME_PORT, 7)
	multiplayer.multiplayer_peer = peer
	i_am_host = true

func start_game(player_order):
	var player_list = []
	for player_id in players:
		var seat_num = player_order.find(players[player_id])
		player_list.append({"id":player_id,"name":players[player_id],"seat":seat_num})
	begin_game.rpc(player_list)

@rpc("any_peer","call_local","reliable")
func begin_game(player_list):
	print("beginning game")
	game = GameScene.instantiate()
	get_node("/root").add_child(game)
	get_node("/root/MainMenu").hide()
	game.seat_players(player_list)

func _on_player_connected(id):
	print("NETWORK EVENT: player connected: " + str(id))
	# send our own info to them to register in their game
	register_player.rpc_id(id, my_name)
	if i_am_host:
		# CPU players wont trigger a _on_player_connected for the client player
		# so the lobby host should send CPU player info to a newly connected client
		for player_id in players:
			if players[player_id].begins_with("CPU"):
				register_cpu.rpc_id(id)

func _on_player_disconnected(id):
	print("NETWORK EVENT: player disconnected: " + str(id))
	unregister_player(id)
	if i_am_host:
		for player_id in players:
			if player_id != 1 && !players[player_id].begins_with("CPU"):
				unregister_player.rpc_id(player_id, id)

# client code

func join_server(host, player_name):
	my_name = player_name
	var peer = ENetMultiplayerPeer.new()
	peer.create_client(host, GAME_PORT)
	multiplayer.multiplayer_peer = peer
	players[multiplayer.get_unique_id()] = my_name

func _on_server_connected():
	print("NETWORK EVENT: server connected")

func _on_server_disconnected():
	print("NETWORK_EVENT: server disconnected")
	close_connections()

# shared code

@rpc("any_peer", "call_remote", "reliable")
func register_player(info):
	var id = multiplayer.get_remote_sender_id()
	print("registering player " + str(info) + " sender was " + str(id))
	players[id] = info
	print("new player: %d" % id)
	connected_players_update.emit(players)

@rpc("any_peer", "call_remote", "reliable")
func unregister_player(id):
	players.erase(id)
	# TODO: re-assign seat numbers?
	print("lost player: %d" % id)
	connected_players_update.emit(players)

func close_connections():
	multiplayer.multiplayer_peer = null
	i_am_host = false
	players = {}
	disconnected_from_host.emit()

@rpc("any_peer", "call_remote", "reliable")
func register_cpu():
	if i_am_host:
		register_cpu.rpc()
	var cpu_count = 0
	for player_id in players:
		if players[player_id].begins_with("CPU"):
			cpu_count = cpu_count + 1
	var cpuid = 10000 + cpu_count + 1
	players[cpuid] = str("CPU:"+str(cpuid-10000))
	connected_players_update.emit(players)
 
@rpc("any_peer", "call_remote", "reliable")
func unregister_cpu():
	if i_am_host:
		unregister_cpu.rpc()
	var cpu_count = 0
	for player_id in players:
		if players[player_id].begins_with("CPU"):
			cpu_count = cpu_count + 1
	var cpuid = 10000 + cpu_count
	players.erase(cpuid)
	connected_players_update.emit(players)


func _ready():
	multiplayer.peer_connected.connect(_on_player_connected)
	multiplayer.connected_to_server.connect(_on_server_connected)
	multiplayer.peer_disconnected.connect(_on_player_disconnected)
	multiplayer.server_disconnected.connect(_on_server_disconnected)

