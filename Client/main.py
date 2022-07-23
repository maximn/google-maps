import json
import random
from aiogram import Bot, types
from aiogram.dispatcher import Dispatcher
from aiogram.types import InlineKeyboardMarkup, InlineKeyboardButton
from aiogram.utils import executor
from PIL import Image
import requests
from io import BytesIO
import urllib3
urllib3.disable_warnings()


bot = Bot(token='5523145562:AAF6lOZN7DQtR5U88OVV8_4Iab84VtpzEXU')
dp = Dispatcher(bot)


class RequestObj:
    def __init__(self):
        self.city = "Одесса"
        self.origin = None
        self.destination = None
        self.mapType = 0

    def serialize(self):
        return {
        "city": self.city,
        "origin": self.origin,
        "destination": self.destination,
        "mapType": self.mapType
    }


users = {}
city_input_flag = False


def get_image(obj: RequestObj):
    response = requests.post(f"https://localhost:7042/api/Maps", json=obj.serialize(), verify=False)
    name = random.randint(0, 100)
    Image.open(BytesIO(response.content)).convert("RGB").save(f'{name}.png')
    return open(f'{name}.png', 'rb')


def get_full(obj: RequestObj):
    response = requests.post(f"https://localhost:7042/api/Maps", json=obj.serialize(), verify=False)
    name = random.randint(0, 100)
    data = response.json()
    image = requests.get(data['img'])

    Image.open(BytesIO(image.content)).convert("RGB").save(f'{name}.png')
    return {"img": open(f'{name}.png', 'rb'), "data": data}


async def send_photo(id, info):
    await bot.send_photo(chat_id=id, photo=info['img'],
                         caption=f"{info['data']['start']}\n{info['data']['end']}\nПримерное время на машине: {info['data']['duration']}")
    users[id].origin = None
    users[id].destination = None


@dp.message_handler(commands=['start'])
async def start(message: types.Message):
    users.update({message.from_user.id: RequestObj()})
    await bot.send_message(chat_id=message.from_user.id,
                           text=f"Hi, type your city")
    global city_input_flag
    city_input_flag = True


@dp.message_handler(commands=['map'])
async def map_type(message: types.Message):
    keys = InlineKeyboardMarkup()
    keys.add(
        InlineKeyboardButton(text='Roadmap', callback_data="0"),
        InlineKeyboardButton(text='Satellite', callback_data="1"),
        InlineKeyboardButton(text='Terrain', callback_data="2"),
        InlineKeyboardButton(text='Hybrid', callback_data="3"))
    await bot.send_message(chat_id=message.from_user.id, text=f"Выберите тип карты", reply_markup=keys)


@dp.callback_query_handler()
async def change_map(call: types.CallbackQuery):
    if call.from_user.id not in users.keys():
        users.update({call.from_user.id: RequestObj()})
    user = users[call.from_user.id]
    user.mapType = int(call.data)
    await bot.send_message(chat_id=call.from_user.id, text=f"Настройки обновлены")


@dp.message_handler(content_types=types.ContentType.LOCATION)
async def location_message(message: types.Message):
    if message.from_user.id not in users.keys():
        users.update({message.from_user.id: RequestObj()})
    user = users[message.from_user.id]
    if user.origin is None:
        user.origin = f"{message.location.latitude}, {message.location.longitude}"
        await bot.send_message(chat_id=message.from_user.id,
                               text=f"Укажите адрес назначения, или отправьте геолокацию")
    else:
        user.destination = f"{message.location.latitude}, {message.location.longitude}"

    if user.origin is not None and user.destination is not None:
        #await bot.send_photo(chat_id=message.from_user.id, photo=get_image(users[message.from_user.id]))
        info = get_full(user)
        await send_photo(message.from_user.id, info)


@dp.message_handler(content_types=types.ContentType.ANY)
async def any_message(message: types.Message):
    global city_input_flag
    if message.from_user.id not in users.keys():
        users.update({message.from_user.id: RequestObj()})
    user = users[message.from_user.id]
    if city_input_flag:
        user.city = message.text
        city_input_flag = False
        await bot.send_message(chat_id=message.from_user.id, text=f"Your city is {message.text}")
        await bot.send_message(chat_id=message.from_user.id,
                               text=f"Укажите адрес отправления, или отправьте геолокацию")

    elif user.origin is None:
        user.origin = message.text
        await bot.send_message(chat_id=message.from_user.id,
                               text=f"Укажите адрес назначения, или отправьте геолокацию")
    elif user.destination is None:
        user.destination = message.text

    if user.origin is not None and user.destination is not None:
        # await bot.send_photo(chat_id=message.from_user.id, photo=get_image(users[message.from_user.id]))
        info = get_full(user)
        await send_photo(message.from_user.id, info)

executor.start_polling(dp, skip_updates=True)
