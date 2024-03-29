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

func host_server():
	var peer = ENetMultiplayerPeer.new()
	peer.create_server(GAME_PORT, 7)
	multiplayer.multiplayer_peer = peer
	i_am_host = true

func start_game(player_order):
	var player_list = []
	for player_id in players:
		var seat_num = player_order.find(player_id)
		player_list.append({"id":player_id,"name":players[player_id]["name"],"seat":seat_num})
	begin_game.rpc(player_list)

# player host code

#func host_game(player_name):
#	my_name = player_name
#	players[1] = {"name":my_name,"ready":false}
#	connected_players_update.emit(players)
#	var peer = ENetMultiplayerPeer.new()
#	peer.create_server(GAME_PORT, 7)
#	multiplayer.multiplayer_peer = peer
#	i_am_host = true

@rpc("any_peer","call_local","reliable")
func toggle_ready():
	var id = multiplayer.get_remote_sender_id()
	print("%d called asdf" % id)
	if id == 0:
		id = multiplayer.get_unique_id()
		#toggle_ready.rpc()
	print("toggling ready for %d from %d to %d" % [id,int(players[id]["ready"]),int(not players[id]["ready"])])
	players[id]["ready"] = not players[id]["ready"]
	if i_am_host:
		if players.values().all(func(p): return p["ready"]):
			print("everyone's ready, lets go")
			start_game(players.keys()) # TODO: restore capability to change seating order
			#var player_order = []
			#for idx in range($GameLobby/PlayerList.get_item_count()/2):
			#	player_order.append($GameLobby/PlayerList.get_item_text(idx*2))
			#Lobby.start_game(player_order)
	else:
		connected_players_update.emit(players)

@rpc("any_peer","call_local","reliable")
func begin_game(player_list):
	print("beginning game")
	game = GameScene.instantiate()
	get_node("/root").add_child(game)
	get_node("/root/MainMenu").hide()
	game.seat_players(player_list)

func _on_player_connected(id):
	print("NETWORK EVENT: player connected: " + str(id))
	if id == 1:
		print(".. actually not a player, this is the server")
	else:
		players[id] = {"name":"???-"+str(id),"ready":false} # to be filled in by register_player rpc calls
	if i_am_host:
		# CPU players wont trigger a _on_player_connected for the client player
		# so the server should send CPU player info to a newly connected client
		var cpu_idx = 0
		for player_id in players:
			if players[player_id]["name"].begins_with("CPU"):
				cpu_idx = cpu_idx + 1
				register_cpu.rpc_id(id, cpu_idx)
	else:
		# send our own info to them to register in their game
		register_player.rpc_id(id, my_name)

func _on_player_disconnected(id):
	print("NETWORK EVENT: player disconnected: " + str(id))
	if id == 1:
		print("..... actuall that was the server")
	else:
		unregister_player(id)
	#if i_am_host:
	#	for player_id in players:
	#		if player_id != 1 && !players[player_id]["name"].begins_with("CPU"):
	#			unregister_player.rpc_id(player_id, id)
	print("players: ")
	for player_id in players:
		print("  > " + str(player_id) + " = " + players[player_id]["name"])


# client code

func join_server(host, player_name):
	my_name = player_name
	var peer = ENetMultiplayerPeer.new()
	peer.create_client(host, GAME_PORT)
	multiplayer.multiplayer_peer = peer
	register_player(my_name) # register myself in my own game
	#players[multiplayer.get_unique_id()] = {"name":my_name,"ready":false}

func _on_server_connected():
	print("NETWORK EVENT: server connected")
	#register_player.rpc_id(1, my_name) <- not needed because we'll do this when we get a peer_connected signal from it

func _on_server_disconnected():
	print("NETWORK_EVENT: server disconnected")
	close_connections()

# shared code

@rpc("any_peer", "call_remote", "reliable")
func register_player(player_name):
	var id = multiplayer.get_remote_sender_id()
	print("registering player remote sender id is " + str(id))
	if id == 0:
		# then this fn was called directly (we're registering ourselves)
		id = multiplayer.get_unique_id()
	print("registering player " + str(player_name) + " sender was " + str(id))
	players[id] = {"name":player_name,"ready":false}
	connected_players_update.emit(players)

@rpc("any_peer", "call_remote", "reliable")
func unregister_player(id):
	print("lost player: %d = %s" % [id, players[id]["name"]])
	players.erase(id)
	# TODO: re-assign seat numbers?
	connected_players_update.emit(players)

func close_connections():
	multiplayer.multiplayer_peer = null
	i_am_host = false
	players = {}
	disconnected_from_host.emit()

func new_cpu():
	var cpu_count = 0
	for player_id in players:
		if players[player_id]["name"].begins_with("CPU"):
			cpu_count = cpu_count + 1
	register_cpu(cpu_count+1)
	register_cpu.rpc(cpu_count+1)

func rem_cpu():
	var cpu_count = 0
	for player_id in players:
		if players[player_id]["name"].begins_with("CPU"):
			cpu_count = cpu_count + 1
	unregister_cpu(cpu_count)
	unregister_cpu.rpc(cpu_count)

@rpc("any_peer", "call_remote", "reliable")
func register_cpu(cpu_idx):
	print("registering cpu idx " + str(cpu_idx))
	var cpuid = 10000 + cpu_idx
	players[cpuid] = {"name":str("CPU:"+str(cpuid-10000)),"ready":true}
	connected_players_update.emit(players)
 
@rpc("any_peer", "call_remote", "reliable")
func unregister_cpu(cpu_idx):
	print("unregistering cpu idx " + str(cpu_idx))
	var cpuid = 10000 + cpu_idx
	players.erase(cpuid)
	connected_players_update.emit(players)


func _ready():
	multiplayer.peer_connected.connect(_on_player_connected)
	multiplayer.connected_to_server.connect(_on_server_connected)
	multiplayer.peer_disconnected.connect(_on_player_disconnected)
	multiplayer.server_disconnected.connect(_on_server_disconnected)

