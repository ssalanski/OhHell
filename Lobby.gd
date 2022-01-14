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
	peer.create_server(GAME_PORT, 4)
	get_tree().network_peer = peer
	i_am_host = true

remote func start_game():
	print("starting game")
	if i_am_host:
		for player_id in players:
			if player_id != 1:
				print("makign rpc call to player: " + str(player_id))
				rpc_id(player_id, "start_game")
	var player_list = []
	var seat_num = 0
	for player_id in players:
		player_list.append({"id":player_id,"name":players[player_id],"seat":seat_num})
		seat_num = seat_num + 1
	game = load("res://Game.tscn").instance()
	game.set_players(player_list)
	get_node("/root").add_child(game)

func send_card(id, card):
	print("sending card %d to %d" % [card,id])
	rpc_id(id, "receive_card", card)
	
func send_trump(card):
	for player_id in players:
		rpc_id(player_id,"receive_trump",card)

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

remote func receive_card(card):
	print("remote received card: %d" % card)
	game.me.receive_card(card)
	game.me.show_hand()

remote func receive_trump(card):
	print("remote received trump: %d" % card)
	game.set_trump(card)

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

