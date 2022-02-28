extends Node

signal connected_players_update(players)
signal disconnected_from_host

const GAME_PORT = 20202

var i_am_host = false
var my_name
var players = {}
var game

# server code

func host_game(player_name):
	my_name = player_name
	players[1] = my_name
	emit_signal("connected_players_update", players)
	var peer = NetworkedMultiplayerENet.new()
	peer.create_server(GAME_PORT, 7)
	get_tree().network_peer = peer
	i_am_host = true

func start_game(player_order):
	var player_list = []
	for player_id in players:
		var seat_num = player_order.find(players[player_id])
		player_list.append({"id":player_id,"name":players[player_id],"seat":seat_num})
	rpc("begin_game", player_list)

remotesync func begin_game(player_list):
	print("beginning game")
	game = load("res://Game.tscn").instance()
	get_node("/root").add_child(game)
	game.seat_players(player_list)

func _on_player_connected(id):
	print("NETWORK EVENT: player connected: " + str(id))
	# send our own info to them to register in their game
	rpc_id(id, "register_player", my_name)
	if i_am_host:
		# CPU players wont trigger a _on_player_connected for the client player
		# so the lobby host should send CPU player info to a newly connected client
		for player_id in players:
			if players[player_id].begins_with("CPU"):
				rpc_id(id, "register_cpu")

func _on_player_disconnected(id):
	print("NETWORK EVENT: player disconnected: " + str(id))
	unregister_player(id)
	if i_am_host:
		for player_id in players:
			if player_id != 1 && !players[player_id].begins_with("CPU"):
				rpc_id(player_id, "unregister_player", id)

# client code

func join_server(host, player_name):
	my_name = player_name
	var peer = NetworkedMultiplayerENet.new()
	peer.create_client(host, GAME_PORT)
	get_tree().network_peer = peer
	players[get_tree().get_network_unique_id()] = my_name

func _on_server_connected():
	print("connected to server")

func _on_server_disconnected():
	print("server disconnected")
	close_connections()

# shared code

remote func register_player(info):
	var id = get_tree().get_rpc_sender_id()
	print("registering player " + str(info) + " sender was " + str(id))
	players[id] = info
	emit_signal("connected_players_update", players)


remote func unregister_player(id):
	#var id = get_tree().get_rpc_sender_id()
	players.erase(id)
	# TODO: re-assign seat numbers?
	print("lost player: %d" % id)
	emit_signal("connected_players_update", players)
	

func close_connections():
	get_tree().network_peer = null
	i_am_host = false
	players = {}
	emit_signal("disconnected_from_host")

remote func register_cpu():
	if i_am_host:
		rpc("register_cpu")
	var cpu_count = 0
	for player_id in players:
		if players[player_id].begins_with("CPU"):
			cpu_count = cpu_count + 1
	var cpuid = 10000 + cpu_count + 1
	players[cpuid] = str("CPU:"+str(cpuid-10000))
	emit_signal("connected_players_update", players)
 
remote func unregister_cpu():
	if i_am_host:
		rpc("unregister_cpu")
	var cpu_count = 0
	for player_id in players:
		if players[player_id].begins_with("CPU"):
			cpu_count = cpu_count + 1
	var cpuid = 10000 + cpu_count
	players.erase(cpuid)
	emit_signal("connected_players_update", players)


func _ready():
	get_tree().connect("network_peer_connected", self, "_on_player_connected")
	get_tree().connect("connected_to_server", self, "_on_server_connected")
	get_tree().connect("network_peer_disconnected", self, "_on_player_disconnected")
	get_tree().connect("server_disconnected", self, "_on_server_disconnected")

