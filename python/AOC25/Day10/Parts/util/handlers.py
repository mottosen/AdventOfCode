import re
from itertools import combinations


# convert an input line to binary data
def line_2_binary(line):
    # build binary number from lights
    def square2bin(elm):
        bin = ""
        for c in elm:
            bin += "1" if c == "#" else "0"

        return (len(bin), int("0b" + bin, 2))

    # build binary numbers from buttons
    def button2bin(line_len, elm):
        bin = list("0" * line_len)

        for c in re.findall(r"\d+", elm):
            bin[int(c)] = "1"

        return int("0b" + "".join(bin), 2)

    bin_len, init_bin = square2bin(re.match(r"\[(.*?)\]", line).group(1))

    # return lights and buttons
    return (
        init_bin,
        [button2bin(bin_len, button) for button in re.findall(r"\((.*?)\)", line)],
    )


# use binary data to turn off lights
def turn_off_lights(line):
    init_bin, buttons = line
    buttons_amount = len(buttons)

    # greedily trying the least buttons at a time
    for presses in range(buttons_amount):
        # brute force combinations of current button press amount
        for limited_buttons in combinations(buttons, presses):
            bin = init_bin

            # press one button at a time
            for button in limited_buttons:
                bin ^= button

            # turning a ligth series fully off is the same problem
            if bin == 0:
                return presses

    return buttons_amount
