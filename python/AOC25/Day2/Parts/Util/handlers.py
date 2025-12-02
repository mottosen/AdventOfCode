def handle_range(handle_id, id_range):
    return list(filter(lambda id: id is not None, map(handle_id, id_range)))
