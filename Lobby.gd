extends Node

signal connected_players_update(players)
signal disconnected_from_host

const GAME_PORT = 20202

var i_am_host = false
var players = {}
var game

# server code

func host_game():
	players[1] = {"name":"me"}
	var peer = NetworkedMultiplayerENet.new()
	peer.create_server(GAME_PORT, 7)
	get_tree().network_peer = peer
	i_am_host = true

func start_game():
	rpc("begin_game")

remotesync func begin_game():
	print("beginning game")
	var player_list = []
	var seat_num = 0
	for player_id in players:
		player_list.append({"id":player_id,"name":players[player_id],"seat":seat_num})
		seat_num = seat_num + 1
	game = load("res://Game.tscn").instance()
	get_node("/root").add_child(game)
	game.seat_players(player_list)

func _on_player_connected(id):
	print("NETWORK EVENT: player connected: " + str(id))
	# register the host player with the client
	register_player(id)
	if i_am_host:
		for player_id in players:
			if player_id != 1:
				rpc_id(player_id, "register_player", id)

func _on_player_disconnected(id):
	print("NETWORK EVENT: player disconnected: " + str(id))
	unregister_player(id)
	if i_am_host:
		for player_id in players:
			if player_id != 1:
				rpc_id(player_id, "unregister_player", id)

# client code

func join_server(host):
	var peer = NetworkedMultiplayerENet.new()
	peer.create_client(host, GAME_PORT)
	get_tree().network_peer = peer

func _on_server_connected():
	print("connected to server")

func _on_server_disconnected():
	print("server disconnected")
	close_connections()

# shared code

remote func register_player(id):
	#var id = get_tree().get_rpc_sender_id()
	players[id] = {"name":str(id)}
	print("new player: %d" % id)
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


func _ready():
	get_tree().connect("network_peer_connected", self, "_on_player_connected")
	get_tree().connect("connected_to_server", self, "_on_server_connected")
	get_tree().connect("network_peer_disconnected", self, "_on_player_disconnected")
	get_tree().connect("server_disconnected", self, "_on_server_disconnected")

