extends Node

signal connected_players_update(players)

const GAME_PORT = 20202

var i_am_host = false
var players = {}

# server code

func host_game():
	players[1] = "me"
	var peer = NetworkedMultiplayerENet.new()
	var e = peer.create_server(GAME_PORT, 4)
	get_tree().network_peer = peer
	i_am_host = true

func start_game():
	pass

func _on_player_connected(id):
	print("NETWORK EVENT: player connected: " + str(id))
	# register the host player with the client
	register_player(id)
	if i_am_host:
		for player in players:
			if player != 1:
				rpc_id(player, "register_player", id)

func _on_player_disconnected(id):
	print("NETWORK EVENT: player disconnected: " + str(id))
	unregister_player(id)
	if i_am_host:
		for player in players:
			if player != 1:
				rpc_id(player, "unregister_player", id)

# client code

func join_server(host):
	var peer = NetworkedMultiplayerENet.new()
	var e = peer.create_client(host, GAME_PORT)
	get_tree().network_peer = peer

func _on_server_connected():
	print("connected to server")

func _on_server_disconnected():
	print("server disconnected")
	close_connections()

# shared code

remote func register_player(id):
	#var id = get_tree().get_rpc_sender_id()
	players[id] = str(id)
	print("new player: %d" % id)
	emit_signal("connected_players_update", players)

remote func unregister_player(id):
	#var id = get_tree().get_rpc_sender_id()
	players.erase(id)
	print("lost player: %d" % id)
	emit_signal("connected_players_update", players)
	

func close_connections():
	get_tree().network_peer = null
	i_am_host = false
	players = {}


func _ready():
	get_tree().connect("network_peer_connected", self, "_on_player_connected")
	get_tree().connect("connected_to_server", self, "_on_server_connected")
	get_tree().connect("network_peer_disconnected", self, "_on_player_disconnected")
	get_tree().connect("server_disconnected", self, "_on_server_disconnected")

