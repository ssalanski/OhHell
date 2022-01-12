extends Node2D


var cards = {}
var trumpSuit = null
var leadSuit = null


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func accept_card(player, card):
	card.position = position.direction_to(player.position) * 50
	card.look_at(player.position)
	card.rotate(PI/2)
	cards[player] = card
	if leadSuit == null:
		leadSuit = card.get_suit()