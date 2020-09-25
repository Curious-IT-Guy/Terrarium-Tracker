from sense_hat import SenseHat
import time
import requests
import json
import datetime

api_link = 'https://terraapi.azurewebsites.net/api/'

current_setting = {}
is_day = False
light_on = False

sense = SenseHat()

green = (0, 255, 0)
yellow = (255, 255, 0)
blue = (0, 0, 255)
red = (255, 0, 0)
white = (255, 255, 255)
nothing = (0, 0, 0)
pink = (255, 105, 180)


# warns
no_warn = True
too_warm = False
too_cold = False
too_humid = False
too_dry = False

# displays
def ok_disp():
	N = nothing
	G = green

	f1 = [
		N, N, N, N, N, N, N, N,
		G, G, G, G, N, G, N, G,
		G, N, N, G, N, G, G, N,
		G, N, N, G, N, G, N, N,
		G, N, N, G, N, G, G, N,
		G, G, G, G, N, G, N, G,
		N, N, N, N, N, N, N, N,
		N, N, N, N, N, N, N, N,
	]

	return f1


def too_warm_disp():
	R = red
	N = nothing

	f1 = [
		N, N, N, N, N, N, N, N,
		N, N, N, R, R, N, N, N,
		N, N, N, R, R, N, N, N,
		N, N, R, R, R, R, N, N,
		N, N, R, R, R, R, N, N,
		N, R, R, R, R, R, R, N,
		N, R, R, R, R, R, R, N,
		N, N, R, R, R, R, N, N,
	]

	return f1


def too_cold_disp():
	O = nothing
	B = blue

	f1 = [
		B, O, O, O, O, O, O, B,
		O, B, O, B, B, O, B, O,
		O, O, B, O, O, B, O, O,
		O, B, O, B, B, O, B, O,
		O, B, O, B, B, O, B, O,
		O, O, B, O, O, B, O, O,
		O, B, O, B, B, O, B, O,
		B, O, O, O, O, O, O, B,
	]

	return f1


def too_humic_disp():
	O = nothing
	B = blue

	f1 = [
		O, O, O, B, B, O, O, O,
		O, O, O, B, B, O, O, O,
		O, O, B, B, B, B, O, O,
		O, O, B, B, B, B, O, O,
		O, B, B, B, B, B, B, O,
		O, B, B, B, B, B, B, O,
		O, O, B, B, B, B, O, O,
		O, O, O, B, B, O, O, O,
	]

	return f1


def too_dry_disp():
	Y = yellow
	N = nothing

	f1 = [
		N, N, N, N, N, N, N, N,
		N, N, N, N, N, N, N, N,
		N, N, N, N, N, N, N, N,
		N, N, N, N, N, N, N, N,
		N, N, N, N, N, N, N, N,
		Y, Y, Y, Y, Y, Y, Y, Y,
		Y, Y, Y, Y, Y, Y, Y, Y,
		Y, Y, Y, Y, Y, Y, Y, Y,
	]

	return f1


def main_loop():
	while True:
		print('Checking status and settings...')
		get_setting()
		post_status()
		check_warns()
		draw_warns()
		time.sleep(60)


def post_status():
	now = datetime.datetime.now()
	current_time = int(str(now.hour)+str(now.minute))
	settings_day_time = int(current_setting['daySettings']['time'].replace(':','')[:-2])
	settings_night_time = int(current_setting['nightSettings']['time'].replace(':','')[:-2])

	global is_day
	is_day = settings_day_time <= current_time < settings_night_time

	global light_on
	if (is_day):
		light_on = current_setting['daySettings']['light']
	else:
		light_on = current_setting['nightSettings']['light']

	data = {'isDay' : is_day, 'temp' : sense.temp, 'humid' : sense.humidity, 'light' : light_on}
	#datadebug = {'isDay': is_day, 'temp': 30.0, 'humid': 33.2, 'light': light_on}
	r = requests.post(url=api_link + 'status/', data=json.dumps(data), headers={'content-type': 'application/json'})

def get_setting():
	resp = json.loads(requests.get(url=api_link + 'settings/').text)
	global current_setting 
	current_setting = resp[0]
	
def check_warns():
	global no_warn
	global too_warm
	global too_cold
	global too_humid
	global too_dry

	current_temp = sense.temp
	current_humid = sense.humid

	##DEBUG TEMP
	#current_temp = 7
	supposed_temp = 0

	##DEBUG HUMID
	#current_humid = 5
	supposed_humid = 0

	if (is_day):
		supposed_temp = current_setting['daySettings']['temp']
		supposed_humid = current_setting['daySettings']['humid']
	else:
		supposed_temp = current_setting['nightSettings']['temp']
		supposed_humid = current_setting['nightSettings']['humid']

	#too hot!!
	if (current_temp > supposed_temp + 5):
		no_warn, too_warm, too_cold, too_humid, too_dry = False, True, False, False, False
	#too cold!!
	elif (current_temp < supposed_temp - 5):
		no_warn, too_warm, too_cold, too_humid, too_dry = False, False, True, False, False
	#too humid!!
	elif (current_humid > supposed_humid + 10):
		no_warn, too_warm, too_cold, too_humid, too_dry = False, False, False, True, False
	#too dry!!
	elif (current_humid < supposed_humid - 10):
		no_warn, too_warm, too_cold, too_humid, too_dry = False, False, False, False, True
	#all good!!
	else:
		no_warn, too_warm, too_cold, too_humid, too_dry = True, False, False, False, False

def draw_warns():
	if (no_warn):
		sense.set_pixels(ok_disp)
	elif (too_warm):
		sense.set_pixels(too_warm_disp)
	elif (too_cold):
		sense.set_pixels(too_cold_disp)
	elif (too_humid):
		sense.set_pixels(too_humic_disp)
	elif (too_dry):
		sense.set_pixels(too_dry_disp)
	
	if (light_on):
		sense.set_pixel(6,0, white)
		sense.set_pixel(7,0, white)
		sense.set_pixel(6,1, white)
		sense.set_pixel(7,1, white)


main_loop()
