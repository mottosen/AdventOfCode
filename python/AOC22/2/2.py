import re

def score_round(e1, a):
    tmp_score = 0
    e2 = 0

    if a == 0: # lose
        e2 = ((e1 + 2) % 3) + 1
    elif a == 1: # draw
        e2 = e1 + 1
        tmp_score = 3
    else: # win
        e2 = ((e1 + 1) % 3) + 1
        tmp_score = 6

    return tmp_score + e2

with open("input.txt", "r") as f:
    lines = f.readlines()

score = 0

for line in lines:
    line = re.match("^([A-C]) ([X-Z])$", line)

    if (line != None):
        (e1, e2) = (ord(line.group(1))-65, ord(line.group(2))-88)
        score += score_round(e1, e2)

print(score)