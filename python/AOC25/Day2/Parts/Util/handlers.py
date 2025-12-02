import re


def handle_range(handle_id, id_range):
    return list(map(handle_id, id_range))


# regex: same number series exactly twice
def handle_id_2(id):
    return id if re.match(r"^(\d+)\1$", str(id)) else 0


# regex: same number series at least twice
def handle_id_n(id):
    return id if re.match(r"^(\d+)\1+$", str(id)) else 0
