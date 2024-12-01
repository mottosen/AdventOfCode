import re

def score_round(e1, e2):
    tmp_score = 0   

    if e1 == e2: # draw
        tmp_score = 3
    elif ((e1 + 1) % 3) == e2: # e2 win
        tmp_score = 6
    else: # e1 win
        tmp_score = 0

    return tmp_score + e2 + 1

with open("test.txt", "r") as f:
    lines = f.readlines()

score = 0

for line in lines:
    line = re.match("^([A-C]) ([X-Z])$", line)

    if (line != None):
        (e1, e2) = (ord(line.group(1))-65, ord(line.group(2))-88)
        score += score_round(e1, e2)

print(score)